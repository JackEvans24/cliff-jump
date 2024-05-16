using System;

namespace CliffJump.Data
{
    [Serializable]
    public class TiltData
    {
        public float InputTilt = 0.5f;
        public float InputMultiplier = 0.5f;
        
        public float LatentTilt = 0.1f;
        public float LatentTiltMultiplier = 0.8f;
        
        public float MaxTiltIncrease = 10f;

        public float FailureAngle = 45f;
    }
}