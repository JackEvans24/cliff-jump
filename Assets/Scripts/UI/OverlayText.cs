using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CliffJump.UI
{
    public class OverlayText : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text label;

        [Header("Animation")]
        [SerializeField] private float fadeInTime = 0.2f;
        [SerializeField] private float sustainTime = 1f;
        [SerializeField] private float fadeOutTime = 0.6f;

        [SerializeField] private float startSize = 0.6f;
        [SerializeField] private float finalSize = 1.5f;

        private Sequence currentSequence;

        public void DisplayText(string text)
        {
            if (currentSequence != null)
            {
                currentSequence.Kill();
            }
            
            canvasGroup.alpha = 0;

            label.transform.localScale = Vector3.one * startSize;
            label.text = text;

            currentSequence = DOTween.Sequence()
                .Append(canvasGroup.DOFade(1f, fadeInTime))
                .Join(label.transform.DOScale(Vector3.one * finalSize, fadeInTime + sustainTime + fadeOutTime))
                .Join(
                    canvasGroup
                        .DOFade(0f, fadeOutTime)
                        .SetDelay(fadeInTime + sustainTime)
                );
        }
    }
}