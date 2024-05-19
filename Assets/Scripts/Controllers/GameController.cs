using CliffJump.Data;
using CliffJump.UI.Views;
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

        [Header("End Game views")]
        [SerializeField] private WinView winView;
        [SerializeField] private GameObject gameOverView;

        private readonly GameResult gameResult = new();

        private void OnEnable()
        {
            runController.RunComplete += OnRunComplete;
            jumpController.JumpSucceeded += OnJumpSucceeded;
            jumpController.JumpFailed += OnJumpFailed;
            aimController.AimComplete += OnAimComplete;
            diveController.TiltSucceeded += OnTiltSucceeded;
            diveController.TiltFailed += OnTiltFailed;
        
            Restart();
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

        public void Restart()
        {
            runController.gameObject.SetActive(false);
            jumpController.gameObject.SetActive(false);
            aimController.gameObject.SetActive(false);
            diveController.gameObject.SetActive(false);
            
            gameOverView.SetActive(false);
            winView.gameObject.SetActive(false);
            
            gameResult.Clear();
            runController.gameObject.SetActive(true);
        }

        private void OnRunComplete(float runSpeed)
        {
            Debug.Log($"RUN COMPLETE: {runSpeed:0.00}");
            gameResult.RunSpeed = runSpeed;
            
            runController.gameObject.SetActive(false);
            jumpController.gameObject.SetActive(true);
        }

        private void OnJumpSucceeded(float timeRemaining)
        {
            Debug.Log($"JUMP COMPLETE: {timeRemaining:0.00}");
            gameResult.QteTimeRemaining = timeRemaining;

            jumpController.gameObject.SetActive(false);
            aimController.gameObject.SetActive(true);
        }

        private void OnJumpFailed()
        {
            Debug.Log($"JUMP FAILED");
            gameOverView.SetActive(true);
        }

        private void OnAimComplete(bool hitObstacle)
        {
            Debug.Log($"AIM COMPLETE, SUCCESS: {!hitObstacle}");
            
            if (hitObstacle)
                gameOverView.SetActive(true);
            else
            {
                aimController.gameObject.SetActive(false);
                diveController.gameObject.SetActive(true);
            }
            
            // TODO: Transition to hit water view on hit
        }

        private void OnTiltSucceeded(float tiltAngle)
        {
            Debug.Log($"TILT SUCCEEDED: {tiltAngle:0.00}");
            gameResult.DiveAngle = tiltAngle;
            
            diveController.gameObject.SetActive(false);
            winView.SetResults(gameResult);
            winView.gameObject.SetActive(true);
            
            // TODO: Transition to clean dive view
        }

        private void OnTiltFailed()
        {
            Debug.Log("TILT FAILED");
            
            gameOverView.SetActive(true);
            
            // TODO: Transition to hit water view
        }
    }
}