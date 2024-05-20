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

        [Header("Sounds")]
        [SerializeField] private SoundTrigger positiveSound;
        [SerializeField] private SoundTrigger negativeSound;
        
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Timings")]
        [SerializeField] private float introDuration = 2f;
        [SerializeField] private float outroDuration = 0.3f;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("Tilt values")]
        [SerializeField] private TiltData tiltData;

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
            tiltListener.Update();
        }

        private void FixedUpdate()
        {
            tiltListener.FixedUpdate();
            view.SetUI(tiltListener.CurrentTiltAmount);
        }

        private void OnTiltFailed()
        {
            feedback.DoNegativeFeedback();
            negativeSound.TriggerSound();
            EndTilt();
            
            StartCoroutine(DoOutro(false));
        }

        private void OnTimerElapsed()
        {
            feedback.DoPositiveFeedback();
            positiveSound.TriggerSound();
            EndTilt();

            var tiltAmount = Math.Abs(tiltListener.CurrentTiltAmount);
            StartCoroutine(DoOutro(true, tiltAmount));
        }

        private IEnumerator DoOutro(bool success, float tiltAngle = 0f)
        {
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
        }
    }
}