using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CiellosAzureDashboard.Pages.Logs
{
    [Authorize(Policy = "IsSuperUser")]
    public class IndexModel : PageModel
    {
        public readonly CADContext _context = new CADContext();
        public IndexModel()
        {
        }
        public IList<Log> Logs { get; set; }
        public void OnGetAsync()
        {
            Logs =  _context.Logs.OrderByDescending(l=> l.logId).ToList();
        }
        public IActionResult OnPostCleanLogs()
        {
            DashboardHelper.CleanLogs(_context);
            return new JsonResult(new { data = "successfull" });
        }
        public async Task<IActionResult> OnPostSetVMLogStateAsync([FromBody] JObject jobject)
        {
            Log log = new Log();
            log.name = jobject["name"].ToString();
            log.timestamp = DateTime.UtcNow;
            log.vmname = jobject["vmname"].ToString();
            log.country = jobject["country"].ToString();
            log.region = jobject["region"].ToString();
            log.resourcegroup = jobject["resourcegroup"].ToString();
            log.city = jobject["city"].ToString();
            log.ip= jobject["ip"].ToString();
            log.key = jobject["key"].ToString();
            log.subscription = jobject["subscription"].ToString();

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            DashboardHelper.CleanLogs(_context);
            return await Task.Run(() => new JsonResult(new { data = "log" }));
        }
        public async Task<IActionResult> OnPostGetVMStartInfoAsync([FromBody] JObject jobject)
        {
            var log = _context.Logs.OrderByDescending(o=>o.timestamp)
                .Where(l => l.key == "Start")
                .FirstOrDefault(l => l.vmname == jobject["vmname"].ToString() && l.resourcegroup == jobject["resourcegroup"].ToString() && l.subscription == jobject["subscription"].ToString());

            return await Task.Run(()=>new JsonResult(log));
        }
        public async Task<IActionResult> OnPostGetVMStopInfoAsync([FromBody] JObject jobject)
        {
            var log = _context.Logs.OrderByDescending(o => o.timestamp)
                .Where(l=> l.key=="Stop")
                .FirstOrDefault(l => l.vmname == jobject["vmname"].ToString() && l.resourcegroup == jobject["resourcegroup"].ToString() && l.subscription == jobject["subscription"].ToString());
            return await Task.Run(() => new JsonResult(log));
        }


    }
}