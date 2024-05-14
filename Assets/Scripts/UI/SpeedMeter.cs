using TMPro;
using UnityEngine;

namespace CliffJump.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class SpeedMeter : MonoBehaviour
    {
        private TMP_Text label;

        private void Awake()
        {
            label = GetComponent<TMP_Text>();
        }

        public void SetSpeedValue(float speed)
        {
            label.text = speed.ToString("0.00") + "km/h";
        }
    }
}