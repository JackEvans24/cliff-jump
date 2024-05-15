using System;
using CliffJump.UI.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class DiveController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DiveView view;
        
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Tilt values")]
        [SerializeField] private float tiltAmount = 0.1f;
        [SerializeField] private float latentTilt = 0.01f;
        [SerializeField] private float latentTiltMultiplier = 0.1f;

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
            Tilt();
            view.SetLabel(currentTiltAmount);
        }

        private void Tilt()
        {
            var inputTip = tiltAmount * currentInput;

            var latentDirection = Math.Sign(currentTiltAmount);
            if (latentDirection == 0)
                latentDirection = 1;

            var latentMultiplier = Math.Max(1f, Math.Abs(currentTiltAmount) * latentTiltMultiplier);

            var latentTip = latentTilt * latentDirection * latentMultiplier;

            currentTiltAmount += inputTip + latentTip;
        }
    }
}