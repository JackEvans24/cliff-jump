using CliffJump.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Data
{
    [CreateAssetMenu(menuName = "Cliff Jump/QTE Action")]
    public class QTEAction : ScriptableObject
    {
        public InputActionReference ActionReference;
        public QTEDirection Direction;
    }
}