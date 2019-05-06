using CiellosAzureDashboard.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Dashboard
    {
        public Dashboard()
        {
        }
        [Key]
        [DisplayName("Dashboard ID")]
        public int DashboardId { get; set; }
        [DisplayName("Dashboard Name")]
        public string DashboardName { get; set; }
        [DisplayName("Dashboard anonymouse code")]
        public string DashboardAnonAccessCode { get; set; }
        [DisplayName("Dashboard Logo URL")]
        public string DashboardLogoUrl { get; set; }
        [DisplayName("Dashboard Connected Applications")]
        public List<DashboardApplication> DashboardApplications { get; set; } = new List<DashboardApplication>();

        [DisplayName("Links")]
        public virtual List<Link> Links { get; set; }
        [DisplayName("Virtual machines display type")]
        public DisplayType DisplayType { get; set; }
        public string ApplicationsDescriptions
        {
            get
            {
                if (DashboardApplications.Count > 0)
                {
                    CADContext context = new CADContext();
                    string[] descriptions = new string[DashboardApplications.Count];
                    int cnt = 0;
                    foreach (var application in context.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d => d.DashboardId == DashboardId).DashboardApplications)
                    {
                        descriptions[cnt] = context.Applications.FirstOrDefault(a=>a.AppId==application.ApplicationId).Description;
                        cnt++;
                    }
                    return string.Join("; ", descriptions);
                }
                else
                {
                    return "";
                }
            }
        }

    }

    public enum DisplayType
    {
        ShowAll = 0,
        SelectType = 1,
        ExcludeType = 2
    }
}
