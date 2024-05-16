using System;
using CliffJump.Data;
using UnityEngine.InputSystem;

namespace CliffJump.Input
{
    public class TiltListener
    {
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
        }

        public void Unlisten()
        {
            tiltAction.Disable();
        }

        public void Update()
        {
            currentInput = tiltAction.ReadValue<float>();
        }

        public void FixedUpdate()
        {
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
        }
    }
}