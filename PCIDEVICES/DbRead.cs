using System;
using System.Management;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace PCIDEVICES
{
    class DbRead
    {
        private static readonly Regex DeviceID = new Regex("DEV_.{4}");
        private static readonly Regex VendorID = new Regex("VEN_.{4}");
        List<Device> devList = new List<Device>();

        public List<Device> SelectPciId()
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(selectQuery);

            foreach (ManagementObject management in Searcher.Get())
            {
                var info= management["DeviceID"].ToString();
                if (info.Contains("PCI"))
                {
                    var dev = new Device(DeviceID.Match(info).Value.Substring(4),
                    VendorID.Match(info).Value.Substring(4));
                    devList.Add(dev);
                }
            }
            return devList;
        }

        public List<Device> GetDevice()
        {
            foreach (var dev in devList)
            {
                var file = new StreamReader("A:\\pci-ids.txt");
                var vendorReg = new Regex("^" + dev.VendorId.ToLower() + "  ");
                var deviceReg = new Regex("^\\t" + dev.DeviceId.ToLower() + "  ");
                while (!file.EndOfStream)
                {
                    var vendorText = file.ReadLine();
                    if (vendorText != null && vendorReg.Match(vendorText).Success)
                    {
                        while (!file.EndOfStream)
                        {
                            var deviceText = file.ReadLine();
                            if (deviceText != null && deviceReg.Match(deviceText).Success)
                            {   
                                dev.VendorName = vendorText.Substring(6);
                                dev.DeviceName= deviceText.Substring(7);
                                break;
                            }
                        }
                    }
                }
            }
            return devList;
        }
    }
}