namespace CliffJump.Data
{
    public class GameResult
    {
        public float RunSpeed;
        public float QteTimeRemaining;
        public float DiveAngle;

        public void Clear()
        {
            RunSpeed = 0f;
            QteTimeRemaining = 0f;
            DiveAngle = 0f;
        }
    }
}