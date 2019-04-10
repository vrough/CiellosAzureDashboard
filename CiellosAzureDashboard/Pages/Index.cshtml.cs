using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CiellosAzureDashboard.Pages
{
    public class IndexModel : PageModel
    {
        public string Title { get; set; }
        public string Logo { get; set; }
        public int DashboardId { get; set; }
        private CADContext _context;
        public IndexModel()
        {
            _context = new CADContext();
        }
        public void OnGet()
        {
            Model.User user = new Model.User();
            if (_context.Users.Where(u=>u.IsSuperUser == true).Count() == 0)
            {
                user.UserName = this.User.Identity.Name;
                user.IsSuperUser = true;
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            if (_context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name) == null)
            {
                
                user.UserName = this.User.Identity.Name;
                user.IsSuperUser = false;
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            user = _context.Users.Include(u => u.Dashboard).FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            if (user.Dashboard != null)
            {
                Title = user.Dashboard.DashboardName;
                Logo = user.Dashboard.DashboardLogoUrl;
                DashboardId = user.Dashboard.DashboardId;
            }
        }


    }
}
