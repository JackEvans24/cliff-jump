using System;
using System.Collections.Generic;
using UnityEngine;

namespace CliffJump.Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("Game components")]
        [SerializeField] private RunController runController;
        [SerializeField] private JumpController jumpController;
        [SerializeField] private AimController aimController;
        [SerializeField] private DiveController diveController;

        [SerializeField] private GameObject gameOverView;

        private readonly Queue<Action> pendingActions = new();

        private void OnEnable()
        {
            runController.RunComplete += OnRunComplete;
            jumpController.JumpSucceeded += OnJumpSucceeded;
            jumpController.JumpFailed += OnJumpFailed;
            aimController.AimComplete += OnAimComplete;
            diveController.TiltSucceeded += OnTiltSucceeded;
            diveController.TiltFailed += OnTiltFailed;
        
            StartGame();
        }

        private void OnDisable()
        {
            runController.RunComplete -= OnRunComplete;
            jumpController.JumpSucceeded -= OnJumpSucceeded;
            jumpController.JumpFailed -= OnJumpFailed;
            aimController.AimComplete -= OnAimComplete;
            diveController.TiltSucceeded -= OnTiltSucceeded;
            diveController.TiltFailed -= OnTiltFailed;
        }

        private void Update()
        {
            while (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();
                action.Invoke();
            }
        }

        public void Restart()
        {
            runController.gameObject.SetActive(false);
            jumpController.gameObject.SetActive(false);
            aimController.gameObject.SetActive(false);
            diveController.gameObject.SetActive(false);
            
            gameOverView.SetActive(false);
            
            StartGame();
        }

        private void StartGame()
        {
            runController.gameObject.SetActive(true);
        }

        private void OnRunComplete(float runSpeed)
        {
            // TODO: Save run speed
            Debug.Log($"RUN COMPLETE: {runSpeed:0.00}");
            
            pendingActions.Enqueue(() => runController.gameObject.SetActive(false));
            pendingActions.Enqueue(() => jumpController.gameObject.SetActive(true));
        }

        private void OnJumpSucceeded(float timeRemaining)
        {
            // TODO: Save QTE time
            Debug.Log($"JUMP COMPLETE: {timeRemaining:0.00}");

            pendingActions.Enqueue(() => jumpController.gameObject.SetActive(false));
            pendingActions.Enqueue(() => aimController.gameObject.SetActive(true));
        }

        private void OnJumpFailed()
        {
            // TODO: straight to game over on fail
            Debug.Log($"JUMP FAILED");
            pendingActions.Enqueue(() => gameOverView.SetActive(true));
        }

        private void OnAimComplete(bool hitObstacle)
        {
            Debug.Log($"AIM COMPLETE, SUCCESS: {!hitObstacle}");

            pendingActions.Enqueue(() => aimController.gameObject.SetActive(false));
            pendingActions.Enqueue(() => diveController.gameObject.SetActive(true));
            
            // TODO: Transition to hit water view on hit
        }

        private void OnTiltSucceeded(float tiltAngle)
        {
            // TODO: Save tilt angle
            Debug.Log($"TILT SUCCEEDED: {tiltAngle:0.00}");
            
            pendingActions.Enqueue(() => diveController.gameObject.SetActive(false));
        }

        private void OnTiltFailed()
        {
            Debug.Log("TILT FAILED");
            
            // TODO: Transition to hit water view
            pendingActions.Enqueue(() => gameOverView.SetActive(true));
        }
    }
}