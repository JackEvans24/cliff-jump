using TMPro;
using UnityEngine;

namespace CliffJump.UI.Views
{
    public class DiveView : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void SetLabel(float diveAngle) => label.text = diveAngle.ToString("0.0");
    }
}