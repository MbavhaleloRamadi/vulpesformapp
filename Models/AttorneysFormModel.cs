using System.ComponentModel.DataAnnotations;

namespace VulpesFormsApp.Models;

public class AttorneysFormModel
{
    [Required(ErrorMessage = "Your name is required")]
    public string YourName { get; set; } = "";

    [Required(ErrorMessage = "Firm name is required")]
    public string FirmName { get; set; } = "";

    [Required(ErrorMessage = "Attorney code is required")]
    public string AttorneyCode { get; set; } = "";

    // — Firm Contacts (dynamic table — sent as JSON string from JS) —
    public string FirmContactsJson { get; set; } = "[]";

    // — Service Level Agreement (checkboxes) —
    public List<string> ServiceLevelAgreement { get; set; } = new();
}