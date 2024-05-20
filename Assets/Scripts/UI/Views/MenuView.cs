using CliffJump.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup ui;
        [SerializeField] private CanvasGroup credits;
        [SerializeField] private Animator character;
        [SerializeField] private TimerPlus timer;

        [Header("Animation")]
        [SerializeField] private float uiFadeTime = 0.1f;
        [SerializeField] private Vector2 randomTriggerBounds;

        [Header("Callbacks")]
        public UnityEvent onStartGameRequested;

        private int triggerIndex;
        private static readonly int Stretch = Animator.StringToHash("Stretch");
        private static readonly int Kick = Animator.StringToHash("Kick");
        private static readonly int Kneel = Animator.StringToHash("Kneel");

        private void OnEnable()
        {
            ui.DOKill();

            credits.alpha = 0f;
            credits.interactable = false;
            credits.blocksRaycasts = false;

            ui.alpha = 1f;
            ui.interactable = true;
            ui.blocksRaycasts = true;

            timer.Elapsed += OnElapsed;
            SetNewTriggerTimer();
        }

        private void OnElapsed()
        {
            character.SetTrigger(triggerIndex == 0 ? Stretch : Kick);

            triggerIndex++;
            triggerIndex %= 2;

            SetNewTriggerTimer();
        }

        private void SetNewTriggerTimer()
        {
            var duration = Random.Range(randomTriggerBounds.x, randomTriggerBounds.y);
            timer.StartTimer(duration, showTimerBar: false);
        }

        public void StartGame()
        {
            timer.Stop();
            timer.Elapsed -= OnElapsed;

            ui.interactable = false;

            character.SetTrigger(Kneel);

            ui.DOFade(0f, uiFadeTime);
        }

        public void StartGameFromAnimation()
        {
            character.ResetTrigger(Kneel);
            
            onStartGameRequested?.Invoke();
        }

        public void ShowCredits()
        {
            ui.interactable = false;
            ui.blocksRaycasts = false;

            ui.DOFade(0f, uiFadeTime);
            credits.DOFade(1f, uiFadeTime).OnComplete(() =>
            {
                credits.interactable = true;
                credits.blocksRaycasts = true;
            });
        }

        public void ShowMenu()
        {
            credits.interactable = false;
            credits.blocksRaycasts = false;

            credits.DOFade(0f, uiFadeTime);
            ui.DOFade(1f, uiFadeTime).OnComplete(() =>
            {
                ui.interactable = true;
                ui.blocksRaycasts = true;
            });
        }
    }
}