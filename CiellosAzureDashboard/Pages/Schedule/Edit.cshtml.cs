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

namespace CiellosAzureDashboard.Pages.Schedule
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
        public Schedules Schedule { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int[] AvailableVMsId { get; set; }
        public SelectList AvailableVMsList { get; set; }

        [BindProperty(SupportsGet = true)]
        public int[] SelectedVmsId { get; set; }
        public SelectList SelectedVMsList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Schedule = await _context.Schedules.Include(d => d.ScheduleVMsList).FirstAsync(d => d.Id == id);
            if (Schedule == null)
            {
                return NotFound();
            }

            SelectedVmsId = Schedule.ScheduleVMsList.Select(a => a.VMId).ToArray();

            List<VM> selVMsList = new List<VM>();
            foreach (var res in SelectedVmsId)
            {
                selVMsList.Add(_context.VMs.Find(res));
            }

            SelectedVMsList = new SelectList(selVMsList, nameof(VM.Id), nameof(VM.VMName));

            var allVM = _context.VMs;
            List<int> arrApp = new List<int>();

            foreach (var app in allVM)
            {
                arrApp.Add(app.Id);
            }
            foreach (var rApp in SelectedVmsId)
            {
                arrApp.Remove(rApp);
            }
            List<VM> avalVMsList = new List<VM>();
            foreach (var res in arrApp)
            {
                avalVMsList.Add(_context.VMs.Find(res));
            }


            AvailableVMsId = arrApp.ToArray();
            AvailableVMsList = new SelectList(avalVMsList, nameof(VM.Id), nameof(VM.VMName));

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {                 
            
            using (var ctx = new CADContext())
            {
                Schedules schedules = ctx.Schedules.Find(id);
                schedules.ScheduleVMsList.Clear();
                schedules.Name = Schedule.Name;
                schedules.StartTime = Schedule.StartTime;
                schedules.StopTime = Schedule.StopTime;

                var sceduleVMs = ctx.ScheduleVM.Where(s => s.ScheduleId == Schedule.Id).ToList();
                foreach (var res in sceduleVMs)
                {
                    ctx.ScheduleVM.Remove(res);
                }

                foreach (var res in SelectedVmsId)
                {
                    var vm = ctx.VMs.Find(res);
                    ScheduleVM scheduleVM = new ScheduleVM();
                    scheduleVM.ScheduleId = 1;
                    scheduleVM.Schedules = Schedule;
                    scheduleVM.VMId = vm.Id;
                    scheduleVM.VM = vm;
                    ctx.ScheduleVM.Add(scheduleVM);
                    Schedule.ScheduleVMsList.Add(scheduleVM);
                }

                ctx.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
        public IActionResult OnPostUpdateLinks([FromBody] JObject res)
        {
            List<VM> vmList = new List<VM>();
            
            var vmId = nameof(VM.Id);
            var vmName = nameof(VM.VMNameRGName);

            return new JsonResult(new { AllVMs = new SelectList(vmList, vmId, vmName)});

        }

    }
}