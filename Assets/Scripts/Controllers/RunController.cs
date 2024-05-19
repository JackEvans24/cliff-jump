using System;
using System.Collections;
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

        [SerializeField] private TimerPlus timer;

        [Header("Intro")]
        [SerializeField] private float introDuration = 2;

        [Header("Outro")]
        [SerializeField] private float outroDuration = 2;
        [SerializeField] private Animator characterAnimator;

        [Header("UI")]
        [SerializeField] private SpeedMeter speedMeter;
        [SerializeField] private TimerBar timerBar;
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private GameObject mashUI;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("RunSpeed")]
        [SerializeField] private float runAcceleration = 2f;
        [SerializeField] private float runDeceleration = 0.05f;
        [SerializeField] private float minRunSpeed = 5f;
        
        [Header("Input")]
        [SerializeField] private InputActionReference[] actionReferences;

        private readonly ButtonMashListener mashListener = new();

        private float currentRunSpeed;
        private float currentDeceleration;
        private float finalRunSpeed;

        private bool triggerOutro;
        
        private static readonly int Outro = Animator.StringToHash("Outro");

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
            
            timer.Elapsed += OnTimerElapsed;
            
            currentRunSpeed = minRunSpeed;
            currentDeceleration = runDeceleration;
            finalRunSpeed = minRunSpeed;
            
            speedMeter.SetSpeedValue(currentRunSpeed);
            overlayText.DisplayText("RUN");

            StartCoroutine(DoIntro());
        }

        private IEnumerator DoIntro()
        {
            yield return new WaitForSeconds(introDuration);

            mashListener.Listen();
            
            mashUI.SetActive(true);
            timerBar.Initialise(timerDuration);

            timer.StartTimer(timerDuration);
        }

        private void FixedUpdate()
        {
            if (triggerOutro)
            {
                triggerOutro = false;
                StartCoroutine(DoOutro());
            }

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

        private void OnTimerElapsed()
        {
            finalRunSpeed = currentRunSpeed;

            mashListener.Unlisten();

            triggerOutro = true;
        }

        private IEnumerator DoOutro()
        {
            mashUI.SetActive(false);
            timerBar.Hide();
            
            characterAnimator.SetTrigger(Outro);

            yield return new WaitForSeconds(outroDuration);
            
            RunComplete?.Invoke(finalRunSpeed);
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
            mashListener.ButtonMashed -= OnMash;
        }
    }
}