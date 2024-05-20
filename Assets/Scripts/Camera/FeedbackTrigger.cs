using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.Camera
{
    public class FeedbackTrigger : MonoBehaviour
    {
        public UnityEvent triggerFeedback;
        public UnityEvent triggerSound;
        public UnityEvent triggerAltSound;

        public void TriggerFeedback() => triggerFeedback?.Invoke();

        public void TriggerSound() => triggerSound?.Invoke();

        public void TriggerAltSound() => triggerAltSound?.Invoke();
    }
}