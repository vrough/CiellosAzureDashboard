using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace CiellosAzureDashboard.Pages
{
    public class AnonymousDashboardModel : PageModel
    {
        private AzureHelper azureHelper;
        private readonly CADContext _context = new CADContext();
        private string _accessCode;
        public string Title { get; set; }
        public string Logo { get; set; }
        public int DashboardId { get; set; }
        public AnonymousDashboardModel(IAzureHelper _azureHelper)
        {
            azureHelper = _azureHelper as AzureHelper;
        }
        public void OnGet(string accessCode)
        {
            var dashboard = azureHelper.GetDashboardByAccessCode(accessCode);
            if (dashboard != null)
            {
                Title = dashboard.DashboardName;
                Logo = dashboard.DashboardLogoUrl;
                DashboardId = dashboard.DashboardId;
            }
        }
        public IActionResult OnPost([FromForm] IFormCollection collection)
        {
            foreach (var key in collection.Keys)
            {
                _accessCode = collection[key.ToString()];

            }
            _accessCode = _accessCode.Replace("?accessCode=","");
            var vmList = azureHelper.GetVirtualMachinesByAccessCode(_accessCode);
            return new JsonResult(new { data = vmList });
        }
    }
}