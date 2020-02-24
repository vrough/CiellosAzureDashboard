using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CiellosAzureDashboard.Model
{
    public class ScheduleVM
    {
        [Key]        
        public int Id { get; set; }
        public int VMId { get; set; }
        public VM VM { get; set; }
        public int ScheduleId { get; set; }
        public Schedules Schedules { get; set; }
    }
}
