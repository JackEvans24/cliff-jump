using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Input
{
    public class QTEListener
    {
        public event Action Progress;
        public event Action Succeeded;
        public event Action Failed;
        
        public bool Enabled { get; private set; }

        private InputAction[] possibleActions;
        private Queue<InputAction> actionQueue;

        public void Listen(InputAction[] actions, Queue<InputAction> qte)
        {
            if (actions.Length <= 0 || qte.Count <= 0)
            {
                Debug.LogWarning("QTE not set up properly");
                return;
            }
            
            possibleActions = actions;
            actionQueue = qte;

            foreach (var action in possibleActions)
            {
                action.Enable();
                action.started += OnStarted;
            }

            Enabled = true;
        }

        public void Unlisten()
        {
            Enabled = false;

            foreach (var action in possibleActions)
            {
                action.started -= OnStarted;
                action.Disable();
            }
        }

        private void OnStarted(InputAction.CallbackContext obj)
        {
            var expectedAction = actionQueue.Peek();
            if (obj.action != expectedAction)
            {
                Failed?.Invoke();
                return;
            }

            actionQueue.Dequeue();

            if (actionQueue.Count <= 0)
            {
                Succeeded?.Invoke();
            }
            else
            {
                Progress?.Invoke();
            }
        }
    }
}
