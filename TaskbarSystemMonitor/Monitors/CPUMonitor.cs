<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Windows7.DesktopIntegration;

namespace TaskbarSystemMonitor.Monitors
{
    class CPUMonitor : PerformanceMonitorBase
    {
        override public float progressVal
        {
            get { return _progressVal; }
            set
            {
                // Dont update if its the same vaue
                if (_progressVal == value) { return; }
                _progressVal = value;

                // Change the 'state' or colour of the bar depending on the progress value
                Windows7Taskbar.ThumbnailProgressState state = Windows7Taskbar.ThumbnailProgressState.Normal;
                if (_progressVal > 50) { state = Windows7Taskbar.ThumbnailProgressState.Paused; }
                if (_progressVal > 80) { state = Windows7Taskbar.ThumbnailProgressState.Error; }
                Windows7Taskbar.SetProgressState(form.Handle, state);

                // Set the progress bar
                Windows7Taskbar.SetProgressValue(form.Handle, (ulong)_progressVal, 100);

                // Set the title text for this window so that we can 
                // have some text statistics too
                form.Text = "CPU: " + _progressVal + "%";
            }
        }  

        override protected PerformanceCounter perfCounter
        {
            get
            {
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Processor";
                pc.CounterName = "% Processor Time";
                pc.InstanceName = "_Total";
                return pc;
            }
        }

        override protected Icon monitorIcon { get { return IconHelper.MakeIcon(Properties.Resources.cpu, 64, true); } }
        override protected string name { get { return "CPU"; } }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Windows7.DesktopIntegration;

namespace TaskbarSystemMonitor.Monitors
{
    class CPUMonitor : PerformanceMonitorBase
    {
        override public float progressVal
        {
            get { return _progressVal; }
            set
            {
                // Dont update if its the same vaue
                if (_progressVal == value) { return; }
                _progressVal = value;

                if (barsEnabled)
                {
                    // Change the 'state' or colour of the bar depending on the progress value
                    var state = Windows7Taskbar.ThumbnailProgressState.Normal;
                    if (_progressVal > 50) { state = Windows7Taskbar.ThumbnailProgressState.Paused; }
                    if (_progressVal > 80) { state = Windows7Taskbar.ThumbnailProgressState.Error; }
                    Windows7Taskbar.SetProgressState(form.Handle, state);

                    // Set the progress bar
                    Windows7Taskbar.SetProgressValue(form.Handle, (ulong)_progressVal, 100);
                }
                else
                {
                    Windows7Taskbar.SetProgressState(form.Handle,  Windows7Taskbar.ThumbnailProgressState.Normal);
                    Windows7Taskbar.SetProgressValue(form.Handle, (ulong)0, 100);
                }

                // Set the title text for this window so that we can 
                // have some text statistics too
                form.Text = _progressVal + "%";
            }
        }  

        override protected PerformanceCounter perfCounter
        {
            get
            {
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Processor";
                pc.CounterName = "% Processor Time";
                pc.InstanceName = "_Total";
                return pc;
            }
        }

        override protected Icon monitorIcon { get { return IconHelper.MakeIcon(Properties.Resources.cpu, 64, true); } }
        override protected string name { get { return "CPU"; } }
    }
}
>>>>>>> v0.4 Essential fixes for windows 8
