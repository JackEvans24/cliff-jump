using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.UI
{
    public class AimReticule : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float smoothing;
        
        [Header("Input")]
        [SerializeField] private InputActionReference movementReference;

        private InputAction movementAction;
        private Vector2 input, movement, currentVelocity;
        
        private void Awake()
        {
            movementAction = movementReference.ToInputAction();
        }

        private void OnEnable()
        {
            movementAction.Enable();
        }

        private void OnDisable()
        {
            movementAction.Disable();
        }

        private void Update()
        {
            input = movementAction.ReadValue<Vector2>();
            movement = Vector2.SmoothDamp(movement, input * moveSpeed, ref currentVelocity, smoothing);
        }

        private void FixedUpdate()
        {
            transform.Translate(movement);
        }
    }
}