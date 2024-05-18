using System;
using System.Timers;

namespace CliffJump.Utilities
{
    public class TimerPlus : Timer
    {
        private DateTime m_dueTime;

        public TimerPlus() => Elapsed += ElapsedAction;

        protected new void Dispose()
        {
            Elapsed -= ElapsedAction;
            base.Dispose();
        }

        public float TimeRemaining => (float)(m_dueTime - DateTime.Now).TotalMilliseconds / 1000f;
        
        public new void Start()
        {
            m_dueTime = DateTime.Now.AddMilliseconds(Interval);
            base.Start();
        }

        private void ElapsedAction(object sender, ElapsedEventArgs e)
        {
            if (AutoReset)
                m_dueTime = DateTime.Now.AddMilliseconds(Interval);
        }
    }
}