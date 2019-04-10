using CiellosAzureDashboard.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{

    public class User
    {
        [Key]
        [DisplayName("User ID")]
        public int UserId { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("User Status")]
        public bool UserStatus { get; set; }
        [DisplayName("Is super user")]
        public bool IsSuperUser { get; set; }
        [DisplayName("Dashboard ID")]
        public virtual Dashboard Dashboard { get; set; }
        public string DashboardDescription
        {
            get
            {
                if (Dashboard != null)
                {
                    CADContext context = new CADContext();
                    return context.Dashboards.FirstOrDefault(d=> d.DashboardId == Dashboard.DashboardId)?.DashboardName;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
