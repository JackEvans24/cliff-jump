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

        public UnityEvent triggerWinScreen;
        public UnityEvent triggerLoseScreen;
        
        public void SetAnimationObjects(ObstacleType obstacleType)
        {
            successObj.SetActive(obstacleType == ObstacleType.None);
            rockObj.SetActive(obstacleType == ObstacleType.Rock || obstacleType == ObstacleType.Cliff);
            boatObj.SetActive(obstacleType == ObstacleType.Boat);
        }

        public void TriggerWin() => triggerWinScreen?.Invoke();

        public void TriggerLose() => triggerLoseScreen?.Invoke();
    }
}