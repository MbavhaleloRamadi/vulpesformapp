using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VulpesFormsApp.Models;
using VulpesFormsApp.Services;
using System.Text.Json;

namespace VulpesFormsApp.Pages;

public class AttorneysPageModel : PageModel
{
    private readonly N8nWebhookService _webhook;

    public AttorneysPageModel(N8nWebhookService webhook) => _webhook = webhook;

    [BindProperty]
    public AttorneysFormModel Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var contacts = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            Form.FirmContactsJson ?? "[]") ?? new();

        var payload = new
        {
            formTitle = "Attorneys",
            submittedAt = DateTime.UtcNow.ToString("o"),
            ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",

            yourName = Form.YourName,
            firmName = Form.FirmName,
            attorneyCode = Form.AttorneyCode,
            serviceLevel = Form.ServiceLevelAgreement.FirstOrDefault() ?? "",

            firmContacts = contacts.Select(c => new
            {
                role = c.GetValueOrDefault("Role", ""),
                name = c.GetValueOrDefault("Name and Surname", ""),
                email = c.GetValueOrDefault("Email", ""),
                phone = c.GetValueOrDefault("Phone Number", "")
            }).ToList(),

            primaryContact = contacts.Count > 0 ? new
            {
                role = contacts[0].GetValueOrDefault("Role", ""),
                name = contacts[0].GetValueOrDefault("Name and Surname", ""),
                email = contacts[0].GetValueOrDefault("Email", ""),
                phone = contacts[0].GetValueOrDefault("Phone Number", "")
            } : new { role = "", name = "", email = "", phone = "" }
        };

        var success = await _webhook.SendToWebhookAsync("Attorneys", payload);
        return RedirectToPage("Confirmation", new { form = "Attorneys", success });
    }
}