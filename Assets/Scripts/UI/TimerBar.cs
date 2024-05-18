using DG.Tweening;
using UnityEngine;

namespace CliffJump.UI
{
    public class TimerBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform parent;
        [SerializeField] private Transform bar;
        [SerializeField] private SpriteRenderer[] sprites;

        [Header("Fade")]
        [SerializeField][Min(0.01f)] private float fadeTime = 0.1f;

        [Header("Shake")]
        [SerializeField][Range(0f, 1f)] private float shakeThreshold = 0.5f;
        [SerializeField] private float shakeStrength = 1f;
        [SerializeField] private float shakeTime = 0.5f;
        
        private float maxTime = 1f;
        private Vector3 scale = Vector3.one;

        private bool shaking;
        private Vector3 shakeRotation;

        public void Initialise(float timerLength)
        {
            maxTime = timerLength;
            shaking = false;
            shakeRotation = new Vector3(0f, 0f, shakeStrength);

            parent.DOKill();

            FadeSprites(true);
            
            UpdateTimer(timerLength);
        }

        public void UpdateTimer(float timeRemaining)
        {
            scale.x = timeRemaining / maxTime;
            bar.localScale = scale;

            if (!shaking && scale.x < shakeThreshold)
            {
                shaking = true;

                DOTween.Sequence()
                    .Append(parent.DORotate(shakeRotation, shakeTime))
                    .Append(parent.DORotate(-shakeRotation, shakeTime * 2))
                    .Append(parent.DORotate(Vector3.zero, shakeTime))
                    .SetLoops(Mathf.RoundToInt(timeRemaining / (shakeTime * 4)), LoopType.Restart);
            }
        }

        public void FadeSprites(bool enable)
        {
            foreach (var spriteRenderer in sprites)
            {
                spriteRenderer.DOKill();
                spriteRenderer.DOFade(enable ? 1f : 0f, fadeTime);
            }
        }
    }
}