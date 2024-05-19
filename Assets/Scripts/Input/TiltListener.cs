using System;
using CliffJump.Data;
using UnityEngine.InputSystem;

namespace CliffJump.Input
{
    public class TiltListener
    {
        public event Action TiltFailed;

        public bool Enabled { get; private set; }
        public float CurrentTiltAmount { get; private set; }
        
        private TiltData tiltData;
        private InputAction tiltAction;

        private float currentInput;

        public void Listen(TiltData data, InputAction action)
        {
            tiltData = data;
            tiltAction = action;
            tiltAction.Enable();

            CurrentTiltAmount = 0f;
            Enabled = true;
        }

        public void Unlisten()
        {
            Enabled = false;
            tiltAction.Disable();
        }

        public void Update()
        {
            if (!Enabled)
                return;

            currentInput = tiltAction.ReadValue<float>();
        }

        public void FixedUpdate()
        {
            if (!Enabled)
                return;

            var absoluteTip = Math.Abs(CurrentTiltAmount);
            var inputTipMultiplier = Math.Max(1f, absoluteTip * tiltData.InputMultiplier);
            var inputTip = tiltData.InputTilt * currentInput * inputTipMultiplier;

            var latentDirection = Math.Sign(CurrentTiltAmount);
            if (latentDirection == 0)
                latentDirection = 1;

            var latentMultiplier = Math.Max(1f, absoluteTip * tiltData.LatentTiltMultiplier);
            var latentTip = tiltData.LatentTilt * latentDirection * latentMultiplier;

            var difference = Math.Clamp(inputTip + latentTip, -tiltData.MaxTiltIncrease, tiltData.MaxTiltIncrease);
            CurrentTiltAmount += difference;
            
            if (CurrentTiltAmount > tiltData.FailureAngle || CurrentTiltAmount < -tiltData.FailureAngle)
                TiltFailed?.Invoke();
        }
    }
}