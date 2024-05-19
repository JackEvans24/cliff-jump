using System;

namespace CliffJump.Data
{
    public class GameResult
    {
        public float RunSpeed;
        public float QteTimeRemaining;
        public float DiveAngle;

        public float JumpScore => QteTimeRemaining * 5f;

        public float FinalScore => RunSpeed * JumpScore / (float)Math.Sqrt(Math.Max(0.001f, DiveAngle));

        public void Clear()
        {
            RunSpeed = 0f;
            QteTimeRemaining = 0f;
            DiveAngle = 0f;
        }
    }
}