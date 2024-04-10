using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dotnet8sample.Pages
{
    public class PreAuthModel : PageModel
    {
        public IActionResult OnPost()
        {
            return RedirectToPage("/Index");
        }
    }
}

