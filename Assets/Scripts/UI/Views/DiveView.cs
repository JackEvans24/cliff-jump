using System.Text;
using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class DiveView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text label;
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
            SetLabel(diveAngle);
            SetIndicatorRotation(diveAngle);
        }

        private void SetLabel(float diveAngle)
        {
            var sb = new StringBuilder();
            
            if (diveAngle < -20f)
                sb.Append(">");
            if (diveAngle < -10f)
                sb.Append(">");
            if (diveAngle < -0f)
                sb.Append("> ");

            sb.Append(diveAngle.ToString("0.0"));
            
            if (diveAngle > 0f)
                sb.Append(" <");
            if (diveAngle > 10f)
                sb.Append("<");
            if (diveAngle > 20f)
                sb.Append("<");

            label.text = sb.ToString();
        }

        private void SetIndicatorRotation(float diveAngle)
        {
            var rotation = 0 + diveAngle;
            indicator.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}