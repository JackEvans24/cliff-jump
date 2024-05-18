using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CliffJump.Input
{
    public class ButtonMashListener
    {
        public event Action ButtonMashed;
        
        public bool Enabled { get; private set; }

        private readonly List<InputAction> actions = new();
        private int currentActionIndex = 0;

        public void AddAction(InputAction action) => actions.Add(action);

        public void Listen()
        {
            if (actions.Count <= 0)
            {
                Debug.LogWarning("Listening to no actions");
                return;
            }

            foreach (var action in actions)
            {
                action.Enable();
                action.started += OnStarted;
            }

            Enabled = true;
        }

        public void Unlisten()
        {
            Enabled = false;

            foreach (var action in actions)
            {
                action.started -= OnStarted;
                action.Disable();
            }
        }

        private void OnStarted(InputAction.CallbackContext obj)
        {
            if (actions.IndexOf(obj.action) != currentActionIndex)
                return;
            
            ButtonMashed?.Invoke();

            currentActionIndex++;
            currentActionIndex %= actions.Count;
        }
    }
}
