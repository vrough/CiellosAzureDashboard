using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CiellosAzureDashboard.Pages.Applications
{
    public class EditModel : PageModel
    {
        private readonly CADContext _context = new CADContext();
        [BindProperty]
        public bool IsSelected { get; set; }

        public EditModel()
        {
        }
        [BindProperty]
        public Application Application { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Application =  _context.Applications.First(a=>a.AppId == id);
            IsSelected = Application.ClientSecret == null;

            if (Application == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Applications.Update(Application);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}