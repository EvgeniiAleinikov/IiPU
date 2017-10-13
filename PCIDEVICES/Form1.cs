using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCIDEVICES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DbRead db = new DbRead();
            db.SelectPciId();
            foreach (var device in db.GetDevice())
            {
                richTextBox1.Text +=" VendorID:" + device.VendorId + " " + " VendorName:" + device.VendorName + " DeviceId:" + device.DeviceId +" DeviceName:"+ device.DeviceName+'\n';
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
