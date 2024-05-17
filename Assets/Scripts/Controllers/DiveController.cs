using System;
using System.Timers;
using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class DiveController : MonoBehaviour
    {
        public event Action<float> TiltSucceeded;
        public event Action TiltFailed;

        [Header("References")]
        [SerializeField] private DiveView view;
        
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("Tilt values")]
        [SerializeField] private TiltData tiltData;

        private readonly TiltListener tiltListener = new();
        private readonly Timer timer = new();

        private void OnEnable()
        {
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;

            view.SetBoundaryPositions(tiltData.FailureAngle);
            
            tiltListener.TiltFailed += OnTiltFailed;
            tiltListener.Listen(tiltData, tilt.ToInputAction());
            
            timer.Start();
        }

        private void OnDisable()
        {
            EndTilt();
        }

        private void Update()
        {
            tiltListener.Update();
        }

        private void FixedUpdate()
        {
            tiltListener.FixedUpdate();
            view.SetUI(tiltListener.CurrentTiltAmount);
        }

        private void OnTiltFailed()
        {
            EndTilt();
            TiltFailed?.Invoke();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            EndTilt();

            var tiltAmount = Math.Abs(tiltListener.CurrentTiltAmount);
            TiltSucceeded?.Invoke(tiltAmount);
        }

        private void EndTilt()
        {
            timer.Elapsed -= OnTimerElapsed;
            timer.Stop();

            tiltListener.Unlisten();
            tiltListener.TiltFailed -= OnTiltFailed;
        }
    }
}