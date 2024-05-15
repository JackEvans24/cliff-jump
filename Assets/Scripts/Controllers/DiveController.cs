using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class DiveController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Tilt values")]
        [SerializeField] private float tiltAmount;

        private InputAction tiltAction;
        
        private float currentInput;
        private float currentTiltAmount;

        private void Awake()
        {
            tiltAction = tilt.ToInputAction();
        }

        private void OnEnable()
        {
            tiltAction.Enable();
        }

        private void OnDisable()
        {
            tiltAction.Disable();
        }

        private void Update()
        {
            currentInput = tiltAction.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            currentTiltAmount += tiltAmount * currentInput;
            Debug.Log(currentTiltAmount);
        }
    }
}