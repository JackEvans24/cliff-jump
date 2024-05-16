using System.Text;
using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class DiveView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Transform indicator;

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