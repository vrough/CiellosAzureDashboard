using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiellosAzureDashboard.Pages.Users
{
    public class CreateModel : PageModel
    {
        public readonly CADContext _context = new CADContext();

        public CreateModel()
        {
        }

        public IActionResult OnGet()
        {
            Dashboards = new SelectList(_context.Dashboards, nameof(Dashboard.DashboardId), nameof(Dashboard.DashboardName));
            return Page();
        }

        [BindProperty]
        public new User User { get; set; }
        [BindProperty]
        public int SelectedDashboard { get; set; }

        public SelectList Dashboards { get; set; }
        #region snippetPost
        public IActionResult OnPost()
        {
            User.Dashboard = _context.Dashboards.FirstOrDefault(d=> d.DashboardId ==  SelectedDashboard);
            _context.Users.Add(User);
             _context.SaveChanges();

            return RedirectToPage("./Index");
        }
        #endregion
    }
}