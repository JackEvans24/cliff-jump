using TMPro;
using UnityEngine;

namespace CliffJump.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SpeedMeter : MonoBehaviour
    {
        private TMP_Text label;

        public void SetSpeedValue(float speed)
        {
            if (label == null)
                label = GetComponent<TMP_Text>();
                
            label.text = speed.ToString("0.00") + "km/h";
        }
    }
}