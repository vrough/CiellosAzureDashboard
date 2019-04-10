using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiellosAzureDashboard.Pages.Applications
{
    public class DeleteModel : PageModel
    {
        private readonly CADContext _context = new CADContext();

        public DeleteModel()
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

            Application = _context.Applications.First(a=>a.AppId == id);

            if (Application == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Application = _context.Applications.First(a => a.AppId == id);

            if (Application != null)
            {
                _context.Remove(Application);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}