using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using TaskbarSystemMonitor.Monitors;
using System.Drawing;
using System.Windows.Forms;
using Windows7.DesktopIntegration;

namespace TaskbarSystemMonitor
{
    public class NetworkMonitor : PerformanceMonitorBase
    {
        // Protecteds
        protected PerformanceCounter[] _nicCounters;
        protected Timer _highestValTimer;
        protected float _currentHighestVal;
        protected float _maxRecorded;
        protected Queue<float> _highstVals;

        override public void Init()
        {
            base.Init();
            
            _highstVals = new Queue<float>();
            for (int i = 0; i < 60; i++) { _highstVals.Enqueue(0); }
            _currentHighestVal = _maxRecorded = 10;

            _highestValTimer = new Timer();
            _highestValTimer.Interval = 1000;
            _highestValTimer.Tick += new EventHandler(onHighestValTimerTick);
            _highestValTimer.Start();
        }

        public void onHighestValTimerTick(object sender, EventArgs eArgs)
        {
            _highstVals.Dequeue();
            _highstVals.Enqueue(_currentHighestVal);

            float highest = 0;
            foreach (float f in _highstVals) { highest = Math.Max(highest, f); }
            _maxRecorded = highest;
            _currentHighestVal = 0;

            Console.WriteLine("_maxRecorded -> " + _maxRecorded);
        }

        override public float progressVal
        {
            set
            {
                // Dont update if its the same vaue
                if (_progressVal == value) { return; }
                _progressVal = value;

                _currentHighestVal = Math.Max(_currentHighestVal, _progressVal);
                int percentVal = (int)((_progressVal / (_currentHighestVal > _maxRecorded ? _currentHighestVal : _maxRecorded)) * 100);

                if (barsEnabled)
                {

                    // Change the 'state' or colour of the bar depending on the progress value
                    Windows7Taskbar.ThumbnailProgressState state = Windows7Taskbar.ThumbnailProgressState.Normal;
                    if (percentVal > 50) { state = Windows7Taskbar.ThumbnailProgressState.Paused; }
                    if (percentVal > 80) { state = Windows7Taskbar.ThumbnailProgressState.Error; }
                    Windows7Taskbar.SetProgressState(form.Handle, state);

                    // Set the progress bar
                    Windows7Taskbar.SetProgressValue(form.Handle, (ulong)percentVal, 100);
                }
                else
                {
                    Windows7Taskbar.SetProgressState(form.Handle, Windows7Taskbar.ThumbnailProgressState.Normal);
                    Windows7Taskbar.SetProgressValue(form.Handle, (ulong)0, 100);
                }

                // Set the title text for this window so that we can 
                // have some text statistics too
                form.Text = percentVal + "% (" + value + "KB/s)";
            }
        }

        override protected PerformanceCounter perfCounter
        {
            get
            {
                _nicCounters = GetNICCounters();
                return null;
            }
        }

        override public void onUpdateTimerTick(object sender, EventArgs eArgs)
        {
            try
            {
                float totalKBs = 0;
                for (int i = 0; i < _nicCounters.Length; i++)
                {
                    totalKBs += (_nicCounters[i].NextValue() / 1024);
                }

                _startVal = progressVal;
                _endVal = totalKBs;
                _age = 0;
            }
            catch (Exception e)
            {
            }
        }

        private PerformanceCounter[] GetNICCounters()
        {
            string[] nics = GetNICInstances(System.Environment.MachineName);
            List<PerformanceCounter> nicCounters = new List<PerformanceCounter>();
            foreach (string nicInstance in nics)
            {
                nicCounters.Add(new PerformanceCounter("Network Interface", "Bytes Total/sec", nicInstance, System.Environment.MachineName));
            }
            return nicCounters.ToArray();
        }

        private string[] GetNICInstances(string machineName)
        {
            string filter = "MS TCP Loopback interface";
            List<string> nics = new List<string>();
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface", machineName);
            if (category.GetInstanceNames() != null)
            {
                foreach (string nic in category.GetInstanceNames())
                {
                    if (!nic.Equals(filter, StringComparison.InvariantCultureIgnoreCase))
                    { nics.Add(nic); }
                }
            }
            return nics.ToArray();
        }

        override public void Destory()
        {
            base.Destory();
            foreach (PerformanceCounter counter in _nicCounters) { counter.Dispose(); }
            _highestValTimer.Stop();
            _highestValTimer.Dispose();
        }

       

        override protected Icon monitorIcon { get { return IconHelper.MakeIcon(Properties.Resources.Network_icon, 64, true); } }
        override protected string name { get { return "MEM"; } }
    }
}
