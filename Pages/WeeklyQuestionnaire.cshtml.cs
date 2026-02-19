using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VulpesFormsApp.Models;
using VulpesFormsApp.Services;
using System.Text.Json;

namespace VulpesFormsApp.Pages;

public class WeeklyQuestionnairePageModel : PageModel
{
    private readonly N8nWebhookService _webhook;

    public WeeklyQuestionnairePageModel(N8nWebhookService webhook) => _webhook = webhook;

    [BindProperty]
    public WeeklyQuestionnaireModel Form { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var reportsRefCodes = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            Form.ReportsRefCodesJson ?? "[]") ?? new();
        var skeletonsRefCodes = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            Form.SkeletonsRefCodesJson ?? "[]") ?? new();
        var addonsRefCodes = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(
            Form.AddonsRefCodesJson ?? "[]") ?? new();

        var payload = new
        {
            formTitle = "Weekly Questionnaire",
            submittedAt = DateTime.UtcNow.ToString("o"),
            ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",

            name = $"{Form.FirstName} {Form.LastName}".Trim(),
            firstName = Form.FirstName,
            lastName = Form.LastName,
            date = FormatDate(Form.DateYear, Form.DateMonth, Form.DateDay),

            reportsCompleted = Form.ReportsCompleted,
            reportsRefCodes = reportsRefCodes,
            skeletonsCompleted = Form.SkeletonsCompleted,
            skeletonsRefCodes = skeletonsRefCodes,
            addonsCompleted = Form.AddonsCompleted,
            addonsRefCodes = addonsRefCodes,

            qualityRating = Form.QualityRating,
            deadlineAdherenceRating = Form.DeadlineRating,
            responsivenessRating = Form.ResponsivenessRating,

            keyAchievement = Form.KeyAchievement,
            challenges = Form.Challenges,
            improvementArea = Form.ImprovementArea,
            suggestions = Form.Suggestions,

            signatureBase64 = Form.SignatureBase64,
            hasSignature = !string.IsNullOrEmpty(Form.SignatureBase64),

            // Pre-compute what the old Data Cleaning node computed
            allRefCodes = reportsRefCodes
                .Concat(skeletonsRefCodes)
                .Concat(addonsRefCodes)
                .Select(r => r.GetValueOrDefault("Vulpes Ref Codes", ""))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList(),
            totalItemsCompleted = Form.ReportsCompleted + Form.SkeletonsCompleted + Form.AddonsCompleted
        };

        var success = await _webhook.SendToWebhookAsync("WeeklyQuestionnaire", payload);
        return RedirectToPage("Confirmation", new { form = "Weekly Questionnaire", success });
    }

    private static string? FormatDate(string? year, string? month, string? day)
    {
        if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(day))
            return null;
        return $"{year}-{month.PadLeft(2, '0')}-{day.PadLeft(2, '0')}";
    }
}