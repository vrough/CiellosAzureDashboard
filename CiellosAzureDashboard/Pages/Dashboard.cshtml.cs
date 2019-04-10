using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace CiellosAzureDashboard.Pages
{
    using CiellosAzureDashboard.Data;
    using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
    using Microsoft.Azure.Management.Samples.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Newtonsoft.Json.Linq;
    using System.Globalization;

    public class DashboardModel : PageModel
    {
        public string Message { get; set; }
        private AzureHelper azureHelper;
        private readonly CADContext DBContext = new CADContext();
        public DashboardModel(IAzureHelper _azureHelper)
        {
            azureHelper = _azureHelper as AzureHelper;
        }
        public void OnGet(string accessCode)
        {
            Message = accessCode;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            return await azureHelper.GetVirtualMachinesByUserAsync(this.User);
        }

        public async Task<IActionResult> OnPostGetVMInfoAsync([FromBody] JObject jobject)
        {
            return await azureHelper.GetJOVirtualMachineAsync(jobject);
        }
        public IActionResult OnPostStartVM([FromBody] JObject jobject)
        {
            var vm = azureHelper.GetVirtualMachine(jobject["vmname"].ToString(), jobject["resourcegroup"].ToString());
            vm.StartAsync();
            return new JsonResult("Started");
        }
        public IActionResult OnPostStopVM([FromBody] JObject jobject)
        {
            var vm = azureHelper.GetVirtualMachine(jobject["vmname"].ToString(), jobject["resourcegroup"].ToString());
            vm.DeallocateAsync();
            return new JsonResult("Stopped");
        }
        public async Task<IActionResult> OnPostSetAsDefaultAsync([FromBody] JObject jobject)
        {

            var dash = DBContext.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d => d.DashboardId == (int)(jobject["id"]));
            var user = DBContext.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            user.Dashboard = dash;

            DBContext.SaveChanges();
            return await Task.Run(() => new JsonResult(new { data = true }));
        }
    }
}
