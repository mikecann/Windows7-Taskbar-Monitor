<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace TaskbarSystemMonitor.Monitors
{
    public class PerformanceMonitorBase
    {
        // Publics
        public Form form;

        // protecteds
        protected PerformanceCounter _pc;
        protected Timer _updateTimer;
        protected Timer _animTimer;
        protected Tweener _progressTween;
        protected float _progressVal;
        protected float _startVal;
        protected float _endVal;
        protected float _age;
        protected float _animTimerUpdateInterval = 100;

        virtual public void Init()
        {
            form.Icon = monitorIcon;

            _pc = perfCounter;

            _updateTimer = new Timer();
            _updateTimer.Interval = timerInterval;
            _updateTimer.Tick += new EventHandler(onUpdateTimerTick);
            _updateTimer.Start();

            _animTimer = new Timer();
            _animTimer.Interval = (int)_animTimerUpdateInterval;
            _animTimer.Tick += new EventHandler(onAnimTimerTick);
            _animTimer.Start();

            // Force initial update
            onUpdateTimerTick(null, null);
        }

        virtual public void onUpdateTimerTick(object sender, EventArgs eArgs)
        {
            _startVal = progressVal;
            _endVal = _pc.NextValue();
            _age = 0;
        }

        public void onAnimTimerTick(object sender, EventArgs eArgs)
        {
            _age += _animTimerUpdateInterval;
            progressVal = (int)(_startVal+((_endVal - _startVal) / timerInterval) * _age);
        }

        virtual public void Destory()
        {
            if (_pc != null) { _pc.Dispose(); }
            _animTimer.Stop();
            _animTimer.Dispose();
            _updateTimer.Stop();
            _updateTimer.Dispose();
        }

        virtual public float progressVal
        {
            get { return _progressVal; }
            set {}
        }

        virtual protected PerformanceCounter perfCounter { get { return null; } }
        virtual protected int timerInterval { get { return 500; } }
        virtual protected string name { get { return ""; } }
        virtual protected Icon monitorIcon { get { return null; } }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace TaskbarSystemMonitor.Monitors
{
    public class PerformanceMonitorBase
    {
        // Publics
        public Form form;

        // protecteds
        protected PerformanceCounter _pc;
        protected Timer _updateTimer;
        protected Timer _animTimer;
        protected Tweener _progressTween;
        protected float _progressVal;
        protected float _startVal;
        protected float _endVal;
        protected float _age;
        protected float _animTimerUpdateInterval = 100;
        protected bool _barsEnabled = true;

        virtual public void Init()
        {
            form.Icon = monitorIcon;

            _pc = perfCounter;

            _updateTimer = new Timer();
            _updateTimer.Interval = timerInterval;
            _updateTimer.Tick += new EventHandler(onUpdateTimerTick);
            _updateTimer.Start();

            _animTimer = new Timer();
            _animTimer.Interval = (int)_animTimerUpdateInterval;
            _animTimer.Tick += new EventHandler(onAnimTimerTick);
            _animTimer.Start();

            // Force initial update
            onUpdateTimerTick(null, null);
        }

        virtual public void onUpdateTimerTick(object sender, EventArgs eArgs)
        {
            _startVal = progressVal;        
            _endVal = (int)_pc.NextValue();
            _age = 0;
        }

        virtual public void onAnimTimerTick(object sender, EventArgs eArgs)
        {
            _age += _animTimerUpdateInterval;
            progressVal = (int)(_startVal+((_endVal - _startVal) / timerInterval) * _age);
        }

        virtual public void Destory()
        {
            if (_pc != null) { _pc.Dispose(); }
            _animTimer.Stop();
            _animTimer.Dispose();
            _updateTimer.Stop();
            _updateTimer.Dispose();
        }

        virtual public float progressVal
        {
            get { return _progressVal; }
            set {}
        }

        virtual protected PerformanceCounter perfCounter { get { return null; } }
        virtual protected int timerInterval { get { return 500; } }
        virtual protected string name { get { return ""; } }
        virtual protected Icon monitorIcon { get { return null; } }
        public bool barsEnabled { get { return _barsEnabled; } set { _barsEnabled = value; _progressVal = -1; } }
    }
}
>>>>>>> v0.4 Essential fixes for windows 8
