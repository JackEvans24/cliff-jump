using CliffJump.Data;
using CliffJump.UI;
using CliffJump.UI.Views;
using UnityEngine;

namespace CliffJump.Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("Game components")]
        [SerializeField] private MenuView menuView;
        [SerializeField] private RunController runController;
        [SerializeField] private JumpController jumpController;
        [SerializeField] private AimController aimController;
        [SerializeField] private DiveController diveController;
        [SerializeField] private SplashController splashController;
        
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
            menuView.gameObject.SetActive(false);
            runController.gameObject.SetActive(false);
            jumpController.gameObject.SetActive(false);
            aimController.gameObject.SetActive(false);
            diveController.gameObject.SetActive(false);
            splashController.gameObject.SetActive(false);
            
            gameOverView.SetActive(false);
            winView.gameObject.SetActive(false);
            
            gameResult.Clear();
            runController.gameObject.SetActive(true);
        }

        private void OnRunComplete(float runSpeed)
        {
            gameResult.RunSpeed = runSpeed;
            
            runController.gameObject.SetActive(false);
            jumpController.gameObject.SetActive(true);
        }

        private void OnJumpSucceeded(float timeRemaining)
        {
            gameResult.QteTimeRemaining = timeRemaining;

            jumpController.gameObject.SetActive(false);
            aimController.gameObject.SetActive(true);
        }

        private void OnJumpFailed()
        {
            gameOverView.SetActive(true);
        }

        private void OnAimComplete(ObstacleType aimResult)
        {
            aimController.gameObject.SetActive(false);
            
            if (aimResult != ObstacleType.None)
                TransitionToSplash(aimResult);
            else
                diveController.gameObject.SetActive(true);
        }

        private void OnTiltSucceeded(float tiltAngle)
        {
            gameResult.DiveAngle = tiltAngle;
            winView.SetResults(gameResult);
            
            diveController.gameObject.SetActive(false);
            TransitionToSplash(ObstacleType.None);
        }

        private void OnTiltFailed()
        {
            diveController.gameObject.SetActive(false);
            TransitionToSplash(ObstacleType.Tilt);
        }

        private void TransitionToSplash(ObstacleType obstacleType)
        {
            splashController.SetAnimationObjects(obstacleType);
            splashController.gameObject.SetActive(true);
        }

        public void ShowWinScreen() => winView.gameObject.SetActive(true);

        public void ShowLoseScreen() => gameOverView.SetActive(true);
    }
}