using System.Collections;
using CliffJump.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvas;
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
            canvas.DOKill();
            
            canvas.alpha = 1f;
            canvas.interactable = true;
            
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

            canvas.interactable = false;

            character.SetTrigger(Kneel);

            canvas.DOFade(0f, uiFadeTime);
        }

        public void StartGameFromAnimation()
        {
            character.ResetTrigger(Kneel);
            
            onStartGameRequested?.Invoke();
        }
    }
}