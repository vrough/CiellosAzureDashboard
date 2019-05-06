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
        public IndexModel()
        {
        }
        public void OnGet()
        {
            Model.User user = new Model.User();
            using (var context = new CADContext())
            {
                if (context.Users.Where(u => u.IsSuperUser == true).Count() == 0)
                {
                    user.UserName = this.User.Identity.Name;
                    user.IsSuperUser = true;
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                if (context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name) == null)
                {

                    user.UserName = this.User.Identity.Name;
                    user.IsSuperUser = false;
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                user = context.Users.Include(u => u.Dashboard).FirstOrDefault(u => u.UserName == this.User.Identity.Name);
                if (user.Dashboard != null)
                {
                    Title = user.Dashboard.DashboardName;
                    Logo = user.Dashboard.DashboardLogoUrl;
                    DashboardId = user.Dashboard.DashboardId;
                }
            }
        }


    }
}