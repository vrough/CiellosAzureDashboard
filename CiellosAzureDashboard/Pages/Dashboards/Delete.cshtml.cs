using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiellosAzureDashboard.Pages.Dashboards
{
    public class DeleteModel : PageModel
    {
        public readonly CADContext _context = new CADContext();

        public DeleteModel()
        {
        }

        [BindProperty]
        public Dashboard Dashboard { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dashboard =  _context.Dashboards.First(d=>d.DashboardId == id);

            if (Dashboard == null)
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

            Dashboard = _context.Dashboards.First(d => d.DashboardId == id);

            if (Dashboard != null)
            {
                _context.Remove(Dashboard);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}