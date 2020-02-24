using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class VM : IEquatable<VM>
    {
        public VM()
        {
        }
        [Key]
        [DisplayName("VM ID")]
        public int Id { get; set; }
        public string VMId { get; set; }
        [DisplayName("VM Name")]
        public string VMName { get; set; }
        public string ResourceGroupName { get; set; }
        public string Tags { get; set; }
        public string SubscriptionId { get; set; }
        public string VMSize { get; set; }
        public string PowerState { get; set; }
        public string ProvisioningState { get; set; }
        public int ApplicationId { get; set; }

        public string VMNameRGName => "VM: " + VMName + ". RG: " + ResourceGroupName;


        public override bool Equals(object obj)
        {
            return obj.GetHashCode() == this.GetHashCode();
        }

        public bool Equals(VM obj)
        {
            return obj.VMId == this.VMId;
        }

        public override int GetHashCode()
        {
            return VMId.GetHashCode();
        }


    }
}
