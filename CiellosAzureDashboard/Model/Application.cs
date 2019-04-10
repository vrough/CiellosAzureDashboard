using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Application
    {
       
        [Key]
        [DisplayName("Application ID")]
        public int AppId { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Client ID")]
        public string ClientId { get; set; }
        [DisplayName("Client Secret")]
        public string ClientSecret { get; set; }
        [DisplayName("Certificate Thumbprint")]
        public string Thumbprint { get; set; }
        [DisplayName("Tenant ID (Domain)")]
        public string TenantId { get; set; }
        [DisplayName("Subscription ID")]
        public string SubscriptionId { get; set; }
        public List<DashboardApplication> DashboardApplications { get; set; } = new List<DashboardApplication>();
    }
}
