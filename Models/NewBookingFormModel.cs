using System.ComponentModel.DataAnnotations;

namespace VulpesFormsApp.Models;

public class NewBookingFormModel
{
    [Required(ErrorMessage = "Please select plaintiff type")]
    public string PlaintiffType { get; set; } = "";  // "New Plaintiff" or "Existing Plaintiff"

    // — Plaintiff Name (compound) —
    public string NamePrefix { get; set; } = "";

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = "";

    public string MiddleName { get; set; } = "";

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = "";

    public string NameSuffix { get; set; } = "";

    // — ID Number (required for new plaintiff) —
    public string PlaintiffIdNumber { get; set; } = "";

    // — Existing Plaintiff fields —
    public string KnowsReference { get; set; } = "";  // "Yes" or "No"
    public string VulpesReferenceNumber { get; set; } = "";
    public string PlaintiffIdForExisting { get; set; } = "";

    // — Assessment —
    public string AssessmentDateMonth { get; set; } = "";
    public string AssessmentDateDay { get; set; } = "";
    public string AssessmentDateYear { get; set; } = "";

    public string Location { get; set; } = "";

    // — Attorney —
    public string AttorneyFirm { get; set; } = "";
    public string AttorneyReference { get; set; } = "";
    public string InstructingAttorneyPrefix { get; set; } = "";
    public string InstructingAttorneyFirst { get; set; } = "";
    public string InstructingAttorneyLast { get; set; } = "";
    public string InstructingDateMonth { get; set; } = "";
    public string InstructingDateDay { get; set; } = "";
    public string InstructingDateYear { get; set; } = "";

    // — Services (dynamic table — sent as JSON string from JS) —
    public string ServicesJson { get; set; } = "[]";

    // — Notes —
    public string Notes { get; set; } = "";
}