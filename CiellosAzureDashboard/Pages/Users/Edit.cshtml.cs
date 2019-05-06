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

namespace CiellosAzureDashboard.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly CADContext _context = new CADContext();


        public EditModel()
        {
        }

        [BindProperty]
        public new User User { get; set; }
        [BindProperty]
        public Dashboard SelectedDashboard { get; set; }

        public SelectList Dashboards { get; set; }
        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dashboards = new SelectList(_context.Dashboards.ToList(), nameof(Dashboard.DashboardId), nameof(Dashboard.DashboardName));

            User = _context.Users.Include(u=>u.Dashboard).FirstOrDefault(u => u.UserId == id);
            SelectedDashboard = User.Dashboard;

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Dashboard = _context.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d=>d.DashboardId == SelectedDashboard.DashboardId);
            _context.Users.Update(User);

            _context.SaveChanges(true);

            return RedirectToPage("./Index");
        }
    }
}