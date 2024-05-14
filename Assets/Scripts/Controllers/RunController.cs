using CliffJump.Input;
using CliffJump.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class RunController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private SpeedMeter speedMeter;

        [Header("RunSpeed")]
        [SerializeField] private float runAcceleration = 2f;
        [SerializeField] private float runDeceleration = 0.05f;
        
        [Header("Input")]
        [SerializeField] private InputActionReference[] actionReferences;

        private readonly ButtonMashListener mashListener = new();

        private float currentRunSpeed;
        private float currentDeceleration;

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
            currentRunSpeed = Mathf.Max(0f, currentRunSpeed - currentDeceleration);
            currentDeceleration += runDeceleration;

            speedMeter.SetSpeedValue(currentRunSpeed);
        }

        private void OnMash()
        {
            currentRunSpeed += runAcceleration / Mathf.Max(currentRunSpeed, 1f);
            currentDeceleration = runDeceleration;
        }
    }
}