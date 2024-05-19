using System;
using CliffJump.UI;
using UnityEngine;

namespace CliffJump.Utilities
{
    public class TimerPlus : MonoBehaviour
    {
        [SerializeField] private TimerBar timerBar;
        
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
            
            timerBar.Initialise(duration);
            timerBar.UpdateTimer(TimeRemaining);
        }

        private void Update()
        {
            if (!timerActive)
                return;

            timerBar.UpdateTimer(TimeRemaining);
            
            if (Time.time > startTime + interval)
                TriggerTimerElapsed();
        }

        private void TriggerTimerElapsed()
        {
            timerActive = false;
            Elapsed?.Invoke();
            
            timerBar.Hide();
        }
    }
}