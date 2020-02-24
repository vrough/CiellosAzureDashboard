using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CiellosAzureDashboard.Pages.Schedule
{
    public class CreateModel : PageModel
    {
        public readonly CADContext _context = new CADContext();

        public CreateModel()
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Schedules Schedules { get; set; }


        #region snippetPost
        public async Task<IActionResult> OnPostAsync()
        {
            _context.Schedules.Add(Schedules);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        #endregion

    }
}