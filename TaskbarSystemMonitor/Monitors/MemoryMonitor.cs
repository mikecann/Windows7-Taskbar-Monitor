using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Windows7.DesktopIntegration;

namespace TaskbarSystemMonitor.Monitors
{
    class MemoryMonitor : PerformanceMonitorBase
    {
        override public float progressVal
        {
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
                form.Text = "MEM: " + _progressVal + "%";
            }
        }  

        override protected PerformanceCounter perfCounter
        {
            get
            {
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Memory";
                pc.CounterName = "% Committed Bytes In Use";
                //pc.InstanceName = "_Total";
                return pc;
            }
        }

        override protected Icon monitorIcon { get { return IconHelper.MakeIcon(Properties.Resources.mem, 64, true); } }
        override protected string name { get { return "MEM"; } }

    }
}
