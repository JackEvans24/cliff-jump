using System;
using System.Timers;
using CliffJump.UI.Views;
using UnityEngine;

namespace CliffJump.Controllers
{
    public class AimController : MonoBehaviour
    {
        public event Action<bool> AimComplete;
        
        [Header("References")]
        [SerializeField] private AimView view;
        
        [Header("Timer")]
        [SerializeField] private float timerDuration = 1.5f;

        private readonly Timer timer = new();

        private void OnEnable()
        {
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;
            
            view.SetUpField();
            
            timer.Start();
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timer.Stop();

            var result = view.ReticuleOverlapsObstacle();
            AimComplete?.Invoke(result);
        }
    }
}