using System.Text;
using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class DiveView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void SetLabel(float diveAngle)
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
    }
}