using System.Timers;
using CliffJump.UI.Views;
using UnityEngine;

namespace CliffJump.Controllers
{
    public class AimController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AimView view;
        
        [Header("Timer")]
        [SerializeField] private float timerDuration = 1.5f;

        private readonly Timer timer = new();

        private void OnEnable()
        {
            view.SetUpField();

            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;

            timer.Start();
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timer.Stop();
            
            Debug.Log($"OVERLAP: {view.ReticuleOverlapsObstacle()}");
        }
    }
}