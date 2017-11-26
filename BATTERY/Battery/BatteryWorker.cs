using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battery
{
    class BatteryWorker
    {
        public bool OnLaunch { get; set; }

        public int PrevTimeout { get; set; }
        public double Percentage { get; set; }

        public string State { get; set; }   
        public string TimeLeft { get; set; }
        public string PrevState { get; set; }

        public void Init()
        {
            OnLaunch = true;//запущена программка или нет
            PrevTimeout = GetCurrentTime();//получаем пред установленное время
            UpdateState();//
        }

        public void SetTimeout(double value)
        {
            var p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = "/c powercfg /x -monitor-timeout-dc " + value/60;
            p.Start();
        }

        public void UpdateState()
        {
            SearchState();
            UpdatePercentageAndSetTimeLeft();
        }

#region private
        private int GetCurrentTime()
        {
            var powercfgOut = StartProcess().StandardOutput.ReadToEnd();
            var reg = new Regex("VIDEOIDLE.*\\n.*\\n.*\\n.*\\n.*\\n.*\\n.*");//получаем текущее время выключения экрана
            var videoidle = reg.Match(powercfgOut).Value;
            var batteryIdle = videoidle.Substring(videoidle.Length - 11).TrimEnd();//возвращает время в стринге
            return Convert.ToInt32(batteryIdle, 16) / 60;
        }

        private Process StartProcess()
        {
            var p = new Process();//инициализируем процесс
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "cmd.exe";//включение командной строки
            p.StartInfo.Arguments = "/c powercfg /q";//командной строки
            p.Start();
            return p;
        }

        private void SearchState()
        {
            State = SystemInformation.PowerStatus.PowerLineStatus.ToString();//получение состояния батареи 
            if (OnLaunch)//установка нужных полей
            {
                PrevState = State;//состояние батареи 
                OnLaunch = false;//флаг батареи 
            }
        }

        private void UpdatePercentageAndSetTimeLeft()
        {
            Percentage = SystemInformation.PowerStatus.BatteryLifePercent * 100;
            if (State == "Offline")
            {
                var temp = SystemInformation.PowerStatus.BatteryLifeRemaining;
                if (temp != -1)
                {
                    var span = new TimeSpan(0, 0, temp);//сколько времени осталось
                    TimeLeft = span.ToString("g");
                }
                else TimeLeft = @"Calculating..";
            }
            else
            {
                TimeLeft = @"Device is connected to AC.";
            }
        }
#endregion
    }
}
