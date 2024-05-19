using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.UI
{
    public class EndGameTrigger : MonoBehaviour
    {
        public UnityEvent triggerGameOverScreen;

        public void TriggerGameOver() => triggerGameOverScreen?.Invoke();
    }
}