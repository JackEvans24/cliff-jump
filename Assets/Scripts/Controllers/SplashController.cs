using CliffJump.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.Controllers
{
    public class SplashController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject successObj;
        [SerializeField] private GameObject rockObj;
        [SerializeField] private GameObject boatObj;
        [SerializeField] private GameObject tiltObj;
        [SerializeField] private GameObject cliffObj;

        public UnityEvent triggerWinScreen;
        public UnityEvent triggerLoseScreen;

        public UnityEvent triggerFeedback;
        
        public void SetAnimationObjects(ObstacleType obstacleType)
        {
            successObj.SetActive(obstacleType == ObstacleType.None);
            rockObj.SetActive(obstacleType == ObstacleType.Rock);
            boatObj.SetActive(obstacleType == ObstacleType.Boat);
            tiltObj.SetActive(obstacleType == ObstacleType.Tilt);
            cliffObj.SetActive(obstacleType == ObstacleType.Cliff);
        }

        public void TriggerWin() => triggerWinScreen?.Invoke();

        public void TriggerLose() => triggerLoseScreen?.Invoke();

        public void TriggerFeedback() => triggerFeedback?.Invoke();
    }
}