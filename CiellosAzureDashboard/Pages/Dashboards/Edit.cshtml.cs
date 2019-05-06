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
using Newtonsoft.Json.Linq;

namespace CiellosAzureDashboard.Pages.Dashboards
{
    public class EditModel : PageModel
    {
        public readonly CADContext _context;
        public IAzureHelper azureHelper;

        public EditModel(IAzureHelper _azureHelper, CADContext context)
        {

            azureHelper = _azureHelper;
            _context = context;
      //      

        }

        [BindProperty]
        public DisplayType SelectedDashboardType { get; set; }

        [BindProperty(SupportsGet = true)]
        public Dashboard Dashboard { get; set; }
        [BindProperty(SupportsGet = true)]
        public int[] SelectedApplications { get; set; }
        public SelectList Applications { get; set; }

        [BindProperty(SupportsGet = true)]
        public int[] AvailableApplications { get; set; }
        public SelectList AllApplications { get; set; }

        [BindProperty(SupportsGet = true)]
        public int[] SelectedVMs { get; set; }
        public SelectList SelectedVMsList { get; set; }
        [BindProperty(SupportsGet = true)]
        public int[] ExcludedVMs { get; set; }
        public SelectList ExcludedVMsList { get; set; }

        [BindProperty(SupportsGet = true)]
        public int[] AllVMs { get; set; }
        public SelectList AllVMsList { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {

            Dashboard = await _context.Dashboards.Include(d => d.DashboardApplications).Include(l => l.Links).FirstAsync(d => d.DashboardId == id);
            AllApplications = new SelectList(_context.Applications.AsNoTracking().ToList(), nameof(Application.AppId), nameof(Application.Description));
            SelectedDashboardType = Dashboard.DisplayType;

            var vmId = nameof(VM.Id);
            var vmName = nameof(VM.VMNameRGName);

            AllVMsList = new SelectList(azureHelper.GetVirtualMashinesList().GetVMsByDash(Dashboard, true).ToList(), vmId, vmName);

            SelectedVMsList = new SelectList(azureHelper.GetVirtualMashinesList().GetActiveVMs(Dashboard.DashboardId).ToList(), vmId, vmName);

            ExcludedVMsList = new SelectList(azureHelper.GetVirtualMashinesList().GetActiveVMs(Dashboard.DashboardId).ToList(), vmId, vmName);

            SelectedApplications = Dashboard.DashboardApplications.Select(a => a.ApplicationId).ToArray();
            List<int> arrApp = new List<int>();
            foreach (var app in AllApplications)
            {
                arrApp.Add(int.Parse(app.Value));
            }
            foreach (var rApp in SelectedApplications)
            {
                arrApp.Remove(rApp);
            }
            
            AvailableApplications = arrApp.ToArray();

            List<Application> sApp = new List<Application>();

            foreach (var app in SelectedApplications)
            {
                sApp.Add(_context.Applications.AsNoTracking().FirstOrDefault(ap => ap.AppId == app));
            }

            Applications = new SelectList(sApp, nameof(Application.AppId), nameof(Application.Description));
            if (Dashboard == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            switch (SelectedDashboardType)
            {
                case DisplayType.ShowAll:
                    {
                        azureHelper.GetVirtualMashinesList().UpdateActiveVMs((int)id, new int[0]);
                        break;
                    }
                case DisplayType.ExcludeType:
                    {
                        azureHelper.GetVirtualMashinesList().UpdateActiveVMs((int)id, ExcludedVMs);
                        break;
                    }
                case DisplayType.SelectType:
                    {
                        azureHelper.GetVirtualMashinesList().UpdateActiveVMs((int)id, SelectedVMs);
                        break;
                    }
            }
            
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
                    dash.DisplayType = SelectedDashboardType;
                    ctx.SaveChanges();
            }
            return RedirectToPage("./Index");
        }
        public IActionResult OnPostUpdateLinks([FromBody] JObject res)
        {
            List<VM> vmList = new List<VM>();
            foreach (JObject jo in res["data"])
            {
                vmList.AddRange(azureHelper.GetVirtualMashinesList().GetVMsByAppID(int.Parse(jo["Key"].ToString())));
            }
            var vmId = nameof(VM.Id);
            var vmName = nameof(VM.VMNameRGName);

            return new JsonResult(new { AllVMs = new SelectList(vmList, vmId, vmName)});

        }

    }
}