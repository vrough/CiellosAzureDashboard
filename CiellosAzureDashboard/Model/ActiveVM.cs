using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class ActiveVM
    {
        [Key]
        public int Id { get; set; }
        public string VMId { get; set; }
        public int DashboardId { get; set; }
    }
}
