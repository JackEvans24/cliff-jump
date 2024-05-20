using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvas;

        [Header("Animation")]
        [SerializeField] private float uiFadeTime = 0.1f;

        [Header("Callbacks")]
        public UnityEvent onStartGameRequested;

        private void OnEnable()
        {
            canvas.DOKill();
            
            canvas.alpha = 1f;
            canvas.interactable = true;
        }

        public void StartGame()
        {
            // TODO: Disable UI
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