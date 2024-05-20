using UnityEngine;

namespace CliffJump.Data
{
    public class GameResult
    {
        public float RunSpeed;
        public float QteTimeRemaining;
        public float DiveAngle;

        public float JumpScore => QteTimeRemaining * 10f;

        private float DiveMultiplier => Mathf.Pow(DiveAngle + 0.25f, 0.5f) - 0.2f;

        public float FinalScore => RunSpeed * JumpScore / DiveMultiplier;

        public void Clear()
        {
            RunSpeed = 0f;
            QteTimeRemaining = 0f;
            DiveAngle = 0f;
        }
    }
}