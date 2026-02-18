using System.ComponentModel.DataAnnotations;

namespace VulpesFormsApp.Models;

public class WeeklyQuestionnaireModel
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = "";

    public string DateMonth { get; set; } = "";
    public string DateDay { get; set; } = "";
    public string DateYear { get; set; } = "";

    // — Work metrics —
    public int ReportsCompleted { get; set; }
    public string ReportsRefCodesJson { get; set; } = "[]";  // JSON from dynamic table

    public int SkeletonsCompleted { get; set; }
    public string SkeletonsRefCodesJson { get; set; } = "[]";

    public int AddonsCompleted { get; set; }
    public string AddonsRefCodesJson { get; set; } = "[]";

    // — Ratings —
    public string QualityRating { get; set; } = "";
    public string DeadlineRating { get; set; } = "";
    public string ResponsivenessRating { get; set; } = "";

    // — Feedback —
    [Required(ErrorMessage = "This field is required")]
    public string KeyAchievement { get; set; } = "";

    [Required(ErrorMessage = "This field is required")]
    public string Challenges { get; set; } = "";

    [Required(ErrorMessage = "This field is required")]
    public string ImprovementArea { get; set; } = "";

    [Required(ErrorMessage = "This field is required")]
    public string Suggestions { get; set; } = "";

    // — Signature (base64 PNG from canvas) —
    public string SignatureBase64 { get; set; } = "";
}