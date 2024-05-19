using System;
using System.Collections;
using System.Collections.Generic;
using CliffJump.UI;
using CliffJump.UI.Views;
using CliffJump.Utilities;
using UnityEngine;

namespace CliffJump.Controllers
{
    public class AimController : MonoBehaviour
    {
        public event Action<bool> AimComplete;
        
        [Header("References")]
        [SerializeField] private AimView view;
        [SerializeField] private TimerPlus timer;

        [Header("UI")]
        [SerializeField] private GameObject reticule;
        [SerializeField] private TimerBar timerBar;
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;

        [Header("Timings")]
        [SerializeField] private float introDuration = 2f;
        [SerializeField] private float outroDuration = .5f;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 1.5f;

        private readonly Queue<Action> pendingActions = new();
        private bool listeningForInput;

        private void OnEnable()
        {
            timer.Elapsed += OnTimerElapsed;
            
            view.SetUpField();
            
            overlayText.DisplayText("AIM");

            StartCoroutine(DoIntro());
        }
        
        private IEnumerator DoIntro()
        {
            yield return new WaitForSeconds(introDuration);

            reticule.SetActive(true);
            timerBar.Initialise(timerDuration);
            timer.StartTimer(timerDuration);

            listeningForInput = true;
        }
        
        private void FixedUpdate()
        {
            if (listeningForInput)
                timerBar.UpdateTimer(timer.TimeRemaining);

            while (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();
                action.Invoke();
            }
        }

        private void OnTimerElapsed()
        {
            listeningForInput = false;

            var result = view.ReticuleOverlapsObstacle();
            Debug.Log($"HIT OBSTACLE: {result}");
            pendingActions.Enqueue(() => StartCoroutine(DoOutro(result)));
        }

        private IEnumerator DoOutro(bool hitObstacle)
        {
            reticule.SetActive(false);
            timerBar.Hide();
            
            if (hitObstacle)
                feedback.DoNegativeFeedback();
            else
                feedback.DoPositiveFeedback();

            yield return new WaitForSeconds(outroDuration);
            AimComplete?.Invoke(hitObstacle);
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
        }
    }
}