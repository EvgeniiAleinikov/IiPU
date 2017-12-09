using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MediaDevices;

namespace Laba_4
{
    class Manager
    {
        public List<Usb> DeviseListCreate()
        {
            List<Usb> usbDevices = new List<Usb>();
            var diskDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable);
            var mtpDrives = MediaDevice.GetDevices();

            foreach (MediaDevice device in mtpDrives)
            {
                device.Connect();
                if (device.DeviceType != DeviceType.Generic)
                {
                    usbDevices.Add(new Usb(device.FriendlyName, null, null, null, true));
                }
            }
            foreach (DriveInfo drive in diskDrives)
            {
                usbDevices.Add(new Usb(drive.Name, Convert(drive.TotalFreeSpace),
                    Convert(drive.TotalSize - drive.TotalFreeSpace),
                    Convert(drive.TotalSize), false));
            }
            return usbDevices;
        }
        private string Convert(long value)
        {
            double megaBytes = (value / 1024) / 1024;
            return megaBytes.ToString() + " mb";
        }
    }
}
