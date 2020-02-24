using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Schedules
    {
        [Key]
        [DisplayName("Schedules ID")]
        public int Id { get; set; }
        [DisplayName("Schedules Name")]
        public string Name { get; set; }
        [DisplayName("Start Time")]
        public TimeSpan StartTime { get; set; }
        [DisplayName("Stop Time")]
        public TimeSpan StopTime { get; set; }

        public List<ScheduleVM> ScheduleVMsList { get; set; } = new List<ScheduleVM>();
    }
}
