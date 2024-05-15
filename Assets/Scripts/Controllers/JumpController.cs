using System.Collections.Generic;
using System.Linq;
using CliffJump.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CliffJump.Controllers
{
    public class JumpController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionReference[] actionReferences;
        [SerializeField] private int qteLength = 3;

        private InputAction[] actions;
        private readonly QTEListener qteListener = new();

        private void Start()
        {
            actions = actionReferences
                .Select(actionRef => actionRef.ToInputAction())
                .ToArray();

            var qte = GenerateQteQueue();
            qteListener.Listen(actions, qte);
        }

        private void OnEnable()
        {
            qteListener.Progress += OnQteProgress;
            qteListener.Succeeded += OnQteSucceeded;
            qteListener.Failed += OnQteFailed;
        }

        private void OnDisable()
        {
            qteListener.Progress -= OnQteProgress;
            qteListener.Succeeded -= OnQteSucceeded;
            qteListener.Failed -= OnQteFailed;
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
        }

        private Queue<InputAction> GenerateQteQueue()
        {
            var qte = new Queue<InputAction>();
            for (var i = 0; i < qteLength; i++)
            {
                var action = actions[Random.Range(0, actions.Length)];
                qte.Enqueue(action);
                
                Debug.Log($"Adding QTE action: {action.name}");
            }

            return qte;
        }
    }
}