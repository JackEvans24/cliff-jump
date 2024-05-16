using UnityEngine;

namespace CliffJump.UI.Views
{
    public class DiveView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform indicator;
        [SerializeField] private Transform maxIndicator;
        [SerializeField] private Transform minIndicator;

        [Header("Tweaks")]
        [SerializeField] private float boundaryTweak = 2.5f;

        public void SetBoundaryPositions(float failureAngle)
        {
            maxIndicator.rotation = Quaternion.Euler(0f, 0f, failureAngle - boundaryTweak);
            minIndicator.rotation = Quaternion.Euler(0f, 0f, -failureAngle + boundaryTweak);
        }

        public void SetUI(float diveAngle)
        {
            SetIndicatorRotation(diveAngle);
        }

        private void SetIndicatorRotation(float diveAngle)
        {
            var rotation = 0 + diveAngle;
            indicator.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}