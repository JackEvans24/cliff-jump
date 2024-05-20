using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.Camera
{
    public class FeedbackTrigger : MonoBehaviour
    {
        public UnityEvent triggerFeedback;

        public void TriggerFeedback() => triggerFeedback?.Invoke();
    }
}