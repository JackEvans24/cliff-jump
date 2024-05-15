using System.Collections.Generic;
using System.Linq;
using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CliffJump.Controllers
{
    public class JumpController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private JumpView view;
        
        [Header("Input")]
        [SerializeField] private int qteLength = 3;
        [SerializeField] private QTEAction[] qteActions;

        private readonly QTEListener qteListener = new();

        private void OnEnable()
        {
            qteListener.Progress += OnQteProgress;
            qteListener.Succeeded += OnQteSucceeded;
            qteListener.Failed += OnQteFailed;
            
            StartQTE();
        }

        private void OnDisable()
        {
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

            var inputActions = GenerateQteQueue();
            qteListener.Listen(actions, inputActions);
        }

        private Queue<InputAction> GenerateQteQueue()
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
            Debug.Log("SUCCESS");
            qteListener.Unlisten();
        }

        private void OnQteFailed()
        {
            Debug.Log("FAILURE");
            qteListener.Unlisten();
        }
    }
}