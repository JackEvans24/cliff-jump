﻿using System.Timers;
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

        [Header("Timer")]
        [SerializeField] private float timerDuration = 5;

        [Header("RunSpeed")]
        [SerializeField] private float runAcceleration = 2f;
        [SerializeField] private float runDeceleration = 0.05f;
        
        [Header("Input")]
        [SerializeField] private InputActionReference[] actionReferences;

        private readonly ButtonMashListener mashListener = new();
        private readonly Timer timer = new ();

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
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;

            mashListener.Listen();

            timer.Start();
            Debug.Log("TIMER STARTED");
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

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            timer.Stop();
            
            // TODO: Emit event ??

            Debug.Log($"FINAL SPEED: {currentRunSpeed}");
            enabled = false;
        }
    }
}