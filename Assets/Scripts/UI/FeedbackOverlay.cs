using System;
using DG.Tweening;
using UnityEngine;

namespace CliffJump.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FeedbackOverlay : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        [Header("Colours")]
        [SerializeField] private Color positiveColour;
        [SerializeField] private Color negativeColour;
        [SerializeField] private Color defaultColour;

        [Header("Animation")]
        [SerializeField] private float fadeTime = 0.05f;

        public void DoPositiveFeedback() => DoFeedback(positiveColour);

        public void DoNegativeFeedback() => DoFeedback(negativeColour);

        private void DoFeedback(Color targetColour)
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            
            spriteRenderer.DOKill();

            DOTween.Sequence()
                .Append(spriteRenderer.DOColor(targetColour, fadeTime))
                .Append(spriteRenderer.DOColor(defaultColour, fadeTime));
        }
    }
}