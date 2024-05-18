using System;
using System.Collections;
using System.Timers;
using CliffJump.Input;
using CliffJump.UI;
using CliffJump.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class RunController : MonoBehaviour
    {
        public event Action<float> RunComplete;

        [Header("Intro")]
        [SerializeField] private float introDuration = 2;

        [Header("UI")]
        [SerializeField] private SpeedMeter speedMeter;
        [SerializeField] private TimerBar timerBar;
        [SerializeField] private OverlayText overlayText;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("RunSpeed")]
        [SerializeField] private float runAcceleration = 2f;
        [SerializeField] private float runDeceleration = 0.05f;
        [SerializeField] private float minRunSpeed = 5f;
        
        [Header("Input")]
        [SerializeField] private InputActionReference[] actionReferences;

        private readonly ButtonMashListener mashListener = new();
        private readonly TimerPlus timer = new();

        private float currentRunSpeed;
        private float currentDeceleration;
        private float finalRunSpeed;

        private void Awake()
        {
            foreach (var actionReference in actionReferences)
            {
                var action = actionReference.ToInputAction();
                mashListener.AddAction(action);
            }
        }

        private void OnEnable()
        {
            mashListener.ButtonMashed += OnMash;
            
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;
            
            currentRunSpeed = minRunSpeed;
            currentDeceleration = runDeceleration;
            finalRunSpeed = minRunSpeed;
            
            speedMeter.SetSpeedValue(currentRunSpeed);
            overlayText.DisplayText("RUN");

            StartCoroutine(StartAfterIntro());
        }

        private void OnDisable()
        {
            mashListener.Unlisten();
            mashListener.ButtonMashed -= OnMash;
            
            timer.Elapsed -= OnTimerElapsed;
        }

        private IEnumerator StartAfterIntro()
        {
            yield return new WaitForSeconds(introDuration);

            mashListener.Listen();
            
            // TODO: Display UI
            
            timerBar.Initialise(timerDuration);

            timer.Start();
        }

        private void FixedUpdate()
        {
            if (!mashListener.Enabled)
                return;

            currentRunSpeed = Mathf.Max(minRunSpeed, currentRunSpeed - currentDeceleration);
            currentDeceleration += runDeceleration;
            
            timerBar.UpdateTimer(timer.TimeRemaining);
            speedMeter.SetSpeedValue(currentRunSpeed);
        }

        private void OnMash()
        {
            currentRunSpeed += runAcceleration / Mathf.Max(currentRunSpeed, 1f);
            currentDeceleration = runDeceleration;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timer.Stop();
            finalRunSpeed = currentRunSpeed;
            
            // TODO: Add outro animation
            
            // TODO: Hide UI
            timerBar.FadeSprites(false);
            
            RunComplete?.Invoke(finalRunSpeed);
        }
    }
}