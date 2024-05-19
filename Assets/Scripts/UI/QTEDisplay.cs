using UnityEngine;

namespace CliffJump.UI
{
    public class QTEDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject upArrow;
        [SerializeField] private GameObject downArrow;
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private GameObject activeIndicator;

        [Header("Sizing")]
        [SerializeField] private float inactiveSize = 0.6f;
        [SerializeField] private float activeSize = 0.8f;
        
        public void Initialise(QTEDirection direction)
        {
            upArrow.SetActive(direction == QTEDirection.Up);
            downArrow.SetActive(direction == QTEDirection.Down);
            leftArrow.SetActive(direction == QTEDirection.Left);
            rightArrow.SetActive(direction == QTEDirection.Right);
        }

        public void SetActive(bool active)
        {
            activeIndicator.SetActive(active);
            transform.localScale = Vector3.one * (active ? activeSize : inactiveSize);
        }
    }

    public enum QTEDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}