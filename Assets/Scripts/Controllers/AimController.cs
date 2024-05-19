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
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;

        [Header("Timings")]
        [SerializeField] private float introDuration = 2f;
        [SerializeField] private float outroDuration = .5f;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 1.5f;

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
            timer.StartTimer(timerDuration);
        }

        private void OnTimerElapsed()
        {
            var hitObstacle = view.ReticuleOverlapsObstacle();
            
            reticule.SetActive(false);

            if (hitObstacle)
                feedback.DoNegativeFeedback();
            else
                feedback.DoPositiveFeedback();

            StartCoroutine(DoOutro(hitObstacle));
        }

        private IEnumerator DoOutro(bool hitObstacle)
        {
            yield return new WaitForSeconds(outroDuration);

            AimComplete?.Invoke(hitObstacle);
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
        }
    }
}