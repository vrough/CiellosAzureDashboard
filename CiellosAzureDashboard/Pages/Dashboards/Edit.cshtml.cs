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
    public class EditModel : PageModel
    {
        public readonly CADContext _context;

        public EditModel(CADContext context)
        {
            _context = new CADContext();
            Applications = new SelectList(_context.Applications.AsNoTracking().ToList(), nameof(Application.AppId), nameof(Application.Description));
        }

        [BindProperty(SupportsGet = true)]
        public Dashboard Dashboard { get; set; }
        [BindProperty(SupportsGet = true)]
        public int[] SelectedApplications { get; set; }
        public SelectList Applications { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            Dashboard = await _context.Dashboards.Include(d => d.DashboardApplications).Include(l => l.Links).FirstAsync(d => d.DashboardId == id);
            SelectedApplications = Dashboard.DashboardApplications.Select(a => a.ApplicationId).ToArray();

            if (Dashboard == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            using (var ctx = new CADContext())
            {
                if (Dashboard.DashboardAnonAccessCode == null)
                {
                    Dashboard.DashboardAnonAccessCode = DashboardHelper.CalculateHash(Dashboard.DashboardName);
                }


                var dash = await ctx.Dashboards.Include(d => d.DashboardApplications).Include(l => l.Links).FirstOrDefaultAsync(d => d.DashboardId == Dashboard.DashboardId);
                dash.DashboardApplications.Clear();
                foreach (var sa in SelectedApplications)
                {
                    DashboardApplication app = new DashboardApplication();
                    app.Dashboard = Dashboard;
                    app.Application = ctx.Applications.First(a => a.AppId == sa);
                    dash.DashboardApplications.Add(app);
                }
                dash.Links.Clear();
                dash.Links = Dashboard.Links;
                dash.DashboardAnonAccessCode = Dashboard.DashboardAnonAccessCode;
                dash.DashboardName = Dashboard.DashboardName;
                dash.DashboardLogoUrl = Dashboard.DashboardLogoUrl;
                ctx.SaveChanges();
            }
            return RedirectToPage("./Index");
        }
        public IActionResult UpdateLinks(Dashboard dashboard)
        {

            return Page();
        }
    }
}