using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MongoDB.Pages
{
    public class CustomMenuModel : PageModel
    {
        private readonly ILogger<CustomMenuModel> _logger;

        public CustomMenuModel(ILogger<CustomMenuModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}