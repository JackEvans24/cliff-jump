using System;
using UnityEngine;

namespace CliffJump.Utilities
{
    public class TimerPlus : MonoBehaviour
    {
        public event Action Elapsed;

        public float TimeRemaining => startTime + interval - Time.time;

        private bool timerActive;
        private float startTime;
        private float interval;

        public void StartTimer(float duration)
        {
            timerActive = true;
            startTime = Time.time;
            interval = duration;
        }

        private void Update()
        {
            if (!timerActive)
                return;
            
            if (Time.time > startTime + interval)
                TriggerTimerElapsed();
        }

        private void TriggerTimerElapsed()
        {
            timerActive = false;
            Elapsed?.Invoke();
        }
    }
}