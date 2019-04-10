using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class DashboardApplication
    {
        [Key]
        public int DashboardId { get; set; }
        public Dashboard Dashboard { get; set; }
        [Key]
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
    }
}
