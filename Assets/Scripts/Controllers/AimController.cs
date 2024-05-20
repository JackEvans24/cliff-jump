using System;
using System.Collections;
using CliffJump.UI;
using CliffJump.UI.Views;
using CliffJump.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.Controllers
{
    public class AimController : MonoBehaviour
    {
        public event Action<ObstacleType> AimComplete;
        
        [Header("References")]
        [SerializeField] private AimView view;
        [SerializeField] private TimerPlus timer;

        [Header("UI")]
        [SerializeField] private GameObject reticule;
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;

        [Header("Sounds")]
        [SerializeField] private SoundTrigger positiveSound;
        [SerializeField] private SoundTrigger negativeSound;

        [Header("Timings")]
        [SerializeField] private float introDuration = 2f;
        [SerializeField] private float outroDuration = .5f;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 1.5f;
        
        public UnityEvent triggerMusicStop;

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
            var aimResult = view.ReticuleOverlapsObstacle();
            
            reticule.SetActive(false);

            if (aimResult == ObstacleType.None)
            {
                feedback.DoPositiveFeedback();
                positiveSound.TriggerSound();
            }
            else
            {
                feedback.DoNegativeFeedback();
                negativeSound.TriggerSound();
                triggerMusicStop?.Invoke();
            }

            StartCoroutine(DoOutro(aimResult));
        }

        private IEnumerator DoOutro(ObstacleType aimResult)
        {
            yield return new WaitForSeconds(outroDuration);

            AimComplete?.Invoke(aimResult);
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;
        }
    }
}