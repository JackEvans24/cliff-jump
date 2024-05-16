using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI.Views;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Controllers
{
    public class DiveController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DiveView view;
        
        [Header("Input")]
        [SerializeField] private InputActionReference tilt;

        [Header("Tilt values")]
        [SerializeField] private TiltData tiltData;

        private readonly TiltListener tiltListener = new();

        private void OnEnable()
        {
            tiltListener.Listen(tiltData, tilt.ToInputAction());
        }

        private void OnDisable()
        {
            tiltListener.Unlisten();
        }

        private void Update()
        {
            tiltListener.Update();
        }

        private void FixedUpdate()
        {
            tiltListener.FixedUpdate();
            view.SetUI(tiltListener.CurrentTiltAmount);
        }
    }
}