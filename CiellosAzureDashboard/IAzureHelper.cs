using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;

namespace CiellosAzureDashboard
{
    public interface IAzureHelper
    {
        VMList GetVirtualMashinesList();
        Dashboard GetDashboardByAccessCode(string _accessCode);
        JsonResult GetJOVirtualMachine(JObject jobject);
        List<VM> GetVirtualMachinesByAccessCode(string _accessCode);
        JsonResult GetVirtualMachinesByUser(ClaimsPrincipal _currentUser);
        Task UpdateAllVirtualMashinesAsync();
        //IDbContextTransaction GetTransaction(CADContext _context);
        //void CommitTransaction();
        //void RollbackTransaction();

    }
}