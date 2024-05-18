using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI.Views;
using CliffJump.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CliffJump.Controllers
{
    public class JumpController : MonoBehaviour
    {
        public event Action<float> JumpSucceeded;
        public event Action JumpFailed;
        
        [Header("References")]
        [SerializeField] private JumpView view;
        
        [Header("Input")]
        [SerializeField] private int qteLength = 3;
        [SerializeField] private QTEAction[] qteActions;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 3;

        private readonly QTEListener qteListener = new();
        private readonly TimerPlus timer = new();

        private void OnEnable()
        {
            timer.Interval = timerDuration * 1000;
            timer.Elapsed += OnTimerElapsed;

            qteListener.Progress += OnQteProgress;
            qteListener.Succeeded += OnQteSucceeded;
            qteListener.Failed += OnQteFailed;
            
            StartQTE();
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;

            qteListener.Unlisten();

            qteListener.Progress -= OnQteProgress;
            qteListener.Succeeded -= OnQteSucceeded;
            qteListener.Failed -= OnQteFailed;
        }

        private void StartQTE()
        {
            var actions = qteActions
                .Select(action => action.ActionReference.ToInputAction())
                .ToArray();
            
            view.ClearView();

            var inputActions = GenerateQTEQueue();
            qteListener.Listen(actions, inputActions);
            
            timer.Start();
        }

        private Queue<InputAction> GenerateQTEQueue()
        {
            var inputActions = new Queue<InputAction>();

            for (var i = 0; i < qteLength; i++)
            {
                var action = qteActions[Random.Range(0, qteActions.Length)];
                inputActions.Enqueue(action.ActionReference.ToInputAction());
                
                view.AddQTELabel(action);
            }

            return inputActions;
        }

        private void OnQteProgress()
        {
            Debug.Log("CORRECT BUTTON PRESS");
        }

        private void OnQteSucceeded()
        {
            var timeRemaining = timer.TimeRemaining;

            Debug.Log($"SUCCESS");
            QTEFinished();
            
            JumpSucceeded?.Invoke(timeRemaining);
        }

        private void OnQteFailed()
        {
            Debug.Log("FAILURE");
            QTEFinished();
            
            JumpFailed?.Invoke();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Debug.Log("TIME'S UP");
            QTEFinished();
            
            JumpFailed?.Invoke();
        }

        private void QTEFinished()
        {
            qteListener.Unlisten();
            timer.Stop();
        }
    }
}