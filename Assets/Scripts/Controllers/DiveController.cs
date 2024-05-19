using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private TimerPlus timer;
        
        [Header("UI")]
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

        private void OnEnable()
        {
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
            
            timer.StartTimer(timerDuration);
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

        private void OnTimerElapsed()
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

            tiltListener.Unlisten();
            tiltListener.TiltFailed -= OnTiltFailed;

            view.SetUIEnabled(false);
        }
    }
}