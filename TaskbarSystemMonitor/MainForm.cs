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

namespace TaskbarSystemMonitor
{
    public partial class MainForm : Form
    {
        // Publics
        public int updateInterval = 500;

        // Protecteds
        protected PerformanceMonitorBase _currentMonitor;

        public MainForm()
        {
            InitializeComponent();            
            Windows7Taskbar.AllowTaskbarWindowMessagesThroughUIPI();
            Windows7Taskbar.SetCurrentProcessAppId("TaskbarManaged");
            setActiveMon(new CPUMonitor());
        }

        private void setActiveMon(PerformanceMonitorBase mon)
        {
            if (_currentMonitor != null) { _currentMonitor.Destory(); }
            _currentMonitor = mon;
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

    }
}
