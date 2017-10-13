using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCIDEVICES
{
    class Device
    {
        public String DeviceId;
        public String DeviceName { get; set; }
        public String VendorId;
        public String VendorName { get; set; }

        public Device(String DivId, String VenId)
        {
            DeviceId = DivId;
            VendorId = VenId;
        }
    }
}
