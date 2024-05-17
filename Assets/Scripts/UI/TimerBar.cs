using DG.Tweening;
using UnityEngine;

namespace CliffJump.UI
{
    public class TimerBar: MonoBehaviour
    {
        [SerializeField] private Transform bar;
        [SerializeField] private Transform background;

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

            background.DOKill();
            
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
                    .Append(background.DORotate(shakeRotation, shakeTime))
                    .Append(background.DORotate(-shakeRotation, shakeTime * 2))
                    .Append(background.DORotate(Vector3.zero, shakeTime))
                    .SetLoops(Mathf.RoundToInt(timeRemaining / (shakeTime * 4)), LoopType.Restart);
            }
        }
    }
}