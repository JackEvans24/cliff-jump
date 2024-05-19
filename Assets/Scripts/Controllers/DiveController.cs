using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI;
using CliffJump.UI.Views;
using CliffJump.Utilities;
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
        [SerializeField] private TimerBar timerBar;
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;
        
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Timings")]
        [SerializeField] private float introDuration = 2f;
        [SerializeField] private float outroDuration = 0.3f;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("Tilt values")]
        [SerializeField] private TiltData tiltData;

        private readonly Queue<Action> pendingActions = new();
        private readonly TiltListener tiltListener = new();
        private readonly TimerPlus timer = new();

        private void OnEnable()
        {
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;

            view.SetBoundaryPositions(tiltData.FailureAngle);
            
            tiltListener.TiltFailed += OnTiltFailed;
            
            overlayText.DisplayText("DIVE");

            StartCoroutine(DoIntro());
        }
        
        private IEnumerator DoIntro()
        {
            yield return new WaitForSeconds(introDuration);

            tiltListener.Listen(tiltData, tilt.ToInputAction());

            view.SetUI(tiltListener.CurrentTiltAmount);
            view.SetUIEnabled(true);
            
            timerBar.Initialise(timerDuration);
            timer.Start();
        }

        private void OnDisable()
        {
            EndTilt();
        }

        private void Update()
        {
            if (!tiltListener.Enabled)
                return;
            
            tiltListener.Update();
            timerBar.UpdateTimer(timer.TimeRemaining);

            while (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();
                action.Invoke();
            }
        }

        private void FixedUpdate()
        {
            tiltListener.FixedUpdate();
            view.SetUI(tiltListener.CurrentTiltAmount);
        }

        private void OnTiltFailed()
        {
            pendingActions.Enqueue(() => StartCoroutine(DoEndTilt(false)));
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var tiltAmount = Math.Abs(tiltListener.CurrentTiltAmount);
            pendingActions.Enqueue(() => StartCoroutine(DoEndTilt(true, tiltAmount)));
        }

        private IEnumerator DoEndTilt(bool success, float tiltAngle = 0f)
        {
            EndTilt();
            
            if (success)
                feedback.DoPositiveFeedback();
            else
                feedback.DoNegativeFeedback();

            yield return new WaitForSeconds(outroDuration);
            
            if (success)
                TiltSucceeded?.Invoke(tiltAngle);
            else
                TiltFailed?.Invoke();
        }

        private void EndTilt()
        {
            timer.Elapsed -= OnTimerElapsed;
            timer.Stop();

            tiltListener.Unlisten();
            tiltListener.TiltFailed -= OnTiltFailed;

            view.SetUIEnabled(false);
            timerBar.Hide();
        }
    }
}