using CliffJump.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class RunController : MonoBehaviour
    {
        [SerializeField] private float runAcceleration = 2f, runDeceleration = 0.05f;
        [SerializeField] private InputActionReference[] actionReferences;

        private ButtonMashListener mashListener = new();

        [SerializeField] private float currentRunSpeed;

        private void Awake()
        {
            foreach (var actionReference in actionReferences)
            {
                var action = actionReference.ToInputAction();
                mashListener.AddAction(action);
            }

            mashListener.ButtonMashed += OnMash;
        }

        private void OnEnable()
        {
            mashListener.Listen();
        }

        private void OnDisable()
        {
            mashListener.Unlisten();
        }

        private void FixedUpdate()
        {
            currentRunSpeed = Mathf.Max(0f, currentRunSpeed - runDeceleration);
        }

        private void OnMash()
        {
            currentRunSpeed += runAcceleration / Mathf.Max(currentRunSpeed, 1f);
        }
    }
}