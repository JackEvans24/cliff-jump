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
        private static readonly int Kick = Animator.StringToHash("Kick");
        private static readonly int Stretch = Animator.StringToHash("Stretch");

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
            character.SetTrigger(triggerIndex == 0 ? Kick : Stretch);

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
            canvas.interactable = false;

            StartCoroutine(DoStartGame());
        }

        private IEnumerator DoStartGame()
        {
            yield return canvas
                .DOFade(0f, uiFadeTime)
                .WaitForCompletion();
            
            onStartGameRequested?.Invoke();
        }
    }
}