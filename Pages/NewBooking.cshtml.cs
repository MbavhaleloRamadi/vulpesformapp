using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VulpesFormsApp.Models;
using VulpesFormsApp.Services;
using System.Text.Json;

namespace VulpesFormsApp.Pages;

public class NewBookingModel : PageModel
{
    private readonly N8nWebhookService _webhook;

    public NewBookingModel(N8nWebhookService webhook) => _webhook = webhook;

    [BindProperty]
    public NewBookingFormModel Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // Build the payload matching what the n8n Data Cleaner outputs
        var services = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            Form.ServicesJson ?? "[]") ?? new();

        var payload = new
        {
            // Metadata
            formTitle = "New Booking Form",
            submittedAt = DateTime.UtcNow.ToString("o"),
            ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",

            // Plaintiff
            plaintiff = new
            {
                type = Form.PlaintiffType,
                isNew = Form.PlaintiffType == "New Plaintiff",
                isExisting = Form.PlaintiffType == "Existing Plaintiff",
                name = new
                {
                    prefix = Form.NamePrefix,
                    first = Form.FirstName,
                    middle = Form.MiddleName,
                    last = Form.LastName,
                    suffix = Form.NameSuffix,
                    full = string.Join(" ", new[] {
                        Form.NamePrefix, Form.FirstName, Form.MiddleName,
                        Form.LastName, Form.NameSuffix
                    }.Where(s => !string.IsNullOrWhiteSpace(s)))
                },
                idNumber = Form.PlaintiffIdNumber,
                knowsReference = Form.KnowsReference == "Yes",
                vulpesReference = Form.VulpesReferenceNumber,
                alternativeReference = Form.PlaintiffIdForExisting
            },

            // Assessment
            assessment = new
            {
                assessmentDate = FormatDate(Form.AssessmentDateYear, Form.AssessmentDateMonth, Form.AssessmentDateDay),
                location = Form.Location,
                notes = Form.Notes
            },

            // Attorney
            attorney = new
            {
                firm = Form.AttorneyFirm,
                reference = Form.AttorneyReference,
                name = new
                {
                    prefix = Form.InstructingAttorneyPrefix,
                    first = Form.InstructingAttorneyFirst,
                    last = Form.InstructingAttorneyLast,
                    full = string.Join(" ", new[] {
                        Form.InstructingAttorneyPrefix,
                        Form.InstructingAttorneyFirst,
                        Form.InstructingAttorneyLast
                    }.Where(s => !string.IsNullOrWhiteSpace(s)))
                },
                instructingDate = FormatDate(Form.InstructingDateYear, Form.InstructingDateMonth, Form.InstructingDateDay)
            },

            // Services
            services = services.Select(s => new
            {
                service = s.GetValueOrDefault("Services", ""),
                active = s.GetValueOrDefault("Active?", "") == "Yes",
                assessmentDate = s.GetValueOrDefault("Assessment Date", ""),
                assessmentTime = s.GetValueOrDefault("Assessment Time", ""),
                reportDate = s.GetValueOrDefault("Report Date", ""),
                reportTime = s.GetValueOrDefault("Report Time", ""),
                notify = s.GetValueOrDefault("Notify Someone", "")
            }).ToList()
        };

        var success = await _webhook.SendToWebhookAsync("NewBookingForm", payload);
        return RedirectToPage("Confirmation", new { form = "New Booking", success });
    }

    private static string? FormatDate(string? year, string? month, string? day)
    {
        if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(day))
            return null;
        return $"{year}-{month.PadLeft(2, '0')}-{day.PadLeft(2, '0')}";
    }
}