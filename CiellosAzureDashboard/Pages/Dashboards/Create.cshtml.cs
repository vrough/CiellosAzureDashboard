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

namespace CiellosAzureDashboard.Pages.Dashboards
{
    public class CreateModel : PageModel
    {
        public readonly CADContext _context = new CADContext();

        public CreateModel()
        {
        }

        public IActionResult OnGet()
        {
            Applications = new SelectList(_context.Applications, nameof(Application.AppId), nameof(Application.Description));
            return Page();
        }

        [BindProperty]
        public Dashboard Dashboard { get; set; }
        [BindProperty]
        public int[] SelectedApplications { get; set; }

        public SelectList Applications { get; set; }
        #region snippetPost
        public IActionResult OnPost()
        {
            if (Dashboard.DashboardAnonAccessCode == null)
            {
                Dashboard.DashboardAnonAccessCode = DashboardHelper.CalculateHash(Dashboard.DashboardName);
            }
            _context.Dashboards.Add(Dashboard);
            _context.SaveChanges();

            using (var ctx = new CADContext())
            {
                var dash = ctx.Dashboards.Include(d => d.DashboardApplications).First(d => d.DashboardId == Dashboard.DashboardId);
                foreach (var sa in SelectedApplications)
                {
                    DashboardApplication app = new DashboardApplication();
                    app.Dashboard = Dashboard;
                    app.Application = ctx.Applications.First(a => a.AppId == sa);
                    dash.DashboardApplications.Add(app);
                }
                ctx.SaveChanges();
            }
            return RedirectToPage("./Index");
        }
        #endregion
    }
}