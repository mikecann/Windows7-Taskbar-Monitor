using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Windows7.DesktopIntegration;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using TaskbarSystemMonitor.Monitors;
using System.Configuration;
using TaskbarSystemMonitor.Properties;

namespace TaskbarSystemMonitor
{
    public partial class MainForm : Form
    {
        // Publics
        public int updateInterval = 500;

        // Protecteds
        protected PerformanceMonitorBase _currentMonitor;
        protected bool _barsEnabled = true;

        public MainForm()
        {
            InitializeComponent();
          
            // Parse any commandline ards
            string[] args = Environment.GetCommandLineArgs();
            string type = "cpu";

            // There are two possible settings for this app
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-type") {type = args[i + 1];}
                else if (args[i] == "-bars") { _barsEnabled = Boolean.Parse(args[i+1]); }
            }

            // Set checkedness from command ards
            checkBox1.Checked = !_barsEnabled;

            // Work out which perf to start on
            PerformanceMonitorBase perf = new CPUMonitor();
            if (type == "net") { perf = new NetworkMonitor(); }
            else if (type == "mem") { perf = new MemoryMonitor(); }   
       
            // Go go go
            Windows7Taskbar.AllowTaskbarWindowMessagesThroughUIPI();
            Windows7Taskbar.SetCurrentProcessAppId("TaskbarManaged");
            setActiveMon(perf);
        }

        private void setActiveMon(PerformanceMonitorBase mon)
        {
            if (_currentMonitor != null) { _currentMonitor.Destory(); }

            _cpuBut.BackColor = _memBut.BackColor = _netBut.BackColor = Color.Transparent;
            if(mon is CPUMonitor){ _cpuBut.BackColor = Color.LightBlue;}
            if (mon is MemoryMonitor) { _memBut.BackColor = Color.LightBlue; }
            if (mon is NetworkMonitor) { _netBut.BackColor = Color.LightBlue; }


            _currentMonitor = mon;
            _currentMonitor.barsEnabled = _barsEnabled;
            _currentMonitor.form = this;
            _currentMonitor.Init();
        }

        private void _memBut_Click(object sender, EventArgs e)
        {
            setActiveMon(new MemoryMonitor());            
        }

        private void _cpuBut_Click(object sender, EventArgs e)
        {          
            setActiveMon(new CPUMonitor());
        }

        private void _netBut_Click(object sender, EventArgs e)
        {
            setActiveMon(new NetworkMonitor());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mikecann.co.uk");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _barsEnabled = !checkBox1.Checked;
            if(_currentMonitor!=null) _currentMonitor.barsEnabled = _barsEnabled;
        }

    }
}
