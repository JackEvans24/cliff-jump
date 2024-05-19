using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class AimReticule : MonoBehaviour
    {
        public Collider2D Collider => coll;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float smoothing;
        
        [Header("Input")]
        [SerializeField] private InputActionReference movementReference;

        [Header("Bounds")]
        [SerializeField] private Vector2 min;
        [SerializeField] private Vector2 max;

        private Collider2D coll;

        private InputAction movementAction;
        private Vector2 input, movement, currentVelocity;
        
        private void Awake()
        {
            coll = GetComponent<Collider2D>();
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

            transform.position = Vector3.Max(min, transform.position);
            transform.position = Vector3.Min(max, transform.position);
        }
    }
}