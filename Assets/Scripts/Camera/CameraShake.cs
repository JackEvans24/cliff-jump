using DG.Tweening;
using UnityEngine;

namespace CliffJump.Camera
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private float strength = 0.1f;
        [SerializeField] private int tremolo = 20;

        private Vector3 origin;

        private void Awake()
        {
            origin = transform.position;
        }

        public void DoShake()
        {
            transform.DOKill();
            transform.position = origin;
            transform.DOShakePosition(duration, strength, tremolo);
        }
    }
}