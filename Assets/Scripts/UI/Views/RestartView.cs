using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CliffJump.UI
{
    public class RestartView : MonoBehaviour
    {
        [SerializeField] private InputActionReference restart;

        private InputAction restartAction;

        public UnityEvent restartRequested;
        
        private void Awake()
        {
            restartAction = restart.ToInputAction();
        }

        private void OnEnable()
        {
            restartAction.started += OnRestartPressed;
            restartAction.Enable();
        }

        private void OnDisable()
        {
            restartAction.Disable();
            restartAction.started -= OnRestartPressed;
        }

        private void OnRestartPressed(InputAction.CallbackContext obj)
        {
            restartRequested?.Invoke();
        }
    }
}