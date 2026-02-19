using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VulpesFormsApp.Pages;

public class ConfirmationModel : PageModel
{
    public string FormName { get; set; } = "";
    public bool Success { get; set; }

    public void OnGet(string form, bool success)
    {
        FormName = form;
        Success = success;
    }
}