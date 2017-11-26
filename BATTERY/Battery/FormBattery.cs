using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battery
{
    public partial class FormBattery : Form
    {
        private readonly BatteryWorker batteryWorker = new BatteryWorker();
        public FormBattery()
        {
            InitializeComponent();
        }

        private void FormBattery_Load(object sender, EventArgs e)
        {
            batteryWorker.Init();
            if (batteryWorker.State == "Online") trackBarBattery.Enabled = false;
            UpdateBatteryStatus(null, null);

            UpdateTimer.Tick += UpdateBatteryStatus;//таймер
            UpdateTimer.Interval = 2000;
            UpdateTimer.Start();//стартует таймер обновления

            trackBarBattery.Value = batteryWorker.PrevTimeout;
            batteryWorker.PrevTimeout = batteryWorker.PrevTimeout == 0 ? 0 : batteryWorker.PrevTimeout;
            numericUpDownBattery.Value = batteryWorker.PrevTimeout * 60;
            labelInMinutes.Text = @"Screen timeout in minutes(only for battery) " + trackBarBattery.Value + @" min now.";
        }

        private void UpdateBatteryStatus(object sender, EventArgs e)
        {
            batteryWorker.UpdateState();
            State.Text = batteryWorker.State;

            Percentage.Text = batteryWorker.Percentage + @"%";
            progressBarBattery.Value = (int)batteryWorker.Percentage;
            timeLeft.Text = batteryWorker.TimeLeft;

            if (batteryWorker.PrevState != State.Text)
            {
                if (batteryWorker.PrevState == "Offline")
                {
                    trackBarBattery.Value = batteryWorker.PrevTimeout;
                    labelInMinutes.Text = @"Screen timeout in minutes(only for battery) " + trackBarBattery.Value + @" min now.";
                    trackBarBattery.Enabled = false;
                }
                else
                {
                    trackBarBattery.Enabled = true;
                }
                batteryWorker.PrevState = State.Text;
            }
        }

        private void trackBarBattery_Scroll(object sender, EventArgs e)
        {
            if (trackBarBattery.Value == 0)
            {
                trackBarBattery.Value = 1;
                labelInMinutes.Text = @"Screen timeout(only for battery) " + numericUpDownBattery.Value + @" seconds now.";                
            }
            else
            {
                labelInMinutes.Text = @"Screen timeout in minutes(only for battery) " + trackBarBattery.Value + @" min now.";
            }
            numericUpDownBattery.Value = trackBarBattery.Value * 60;
            batteryWorker.SetTimeout(Convert.ToInt32(numericUpDownBattery.Value));    
        }

        private void numericUpDownBattery_ValueChanged(object sender, EventArgs e)
        {
            labelInMinutes.Text = @"Screen timeout(only for battery) " + numericUpDownBattery.Value + @" seconds now.";
            trackBarBattery.Value = Convert.ToInt32(numericUpDownBattery.Value / 60);//приведение к инту
            batteryWorker.SetTimeout(Convert.ToInt32(numericUpDownBattery.Value));//посылаем время для установки
        }

        private void FormBattery_FormClosing(object sender, FormClosingEventArgs e)
        {
            //batteryWorker.SetTimeout(Convert.ToInt32(numericUpDownBattery.Value));//устанавливаем последнее значение перед выходом
            numericUpDownBattery.Value = batteryWorker.PrevTimeout;
        }
    }
}
