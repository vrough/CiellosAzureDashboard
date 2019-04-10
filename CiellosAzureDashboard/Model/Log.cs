using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Log
    {
        [Key]
        public int logId { get; set; }
        public DateTime timestamp { get; set; }
        public string resourcegroup { get; set; }
        public string vmname { get; set; }
        public string subscription { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string country { get; set; }
    }
}
