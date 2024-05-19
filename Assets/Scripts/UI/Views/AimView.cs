using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CliffJump.UI.Views
{
    public class AimView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AimReticule reticule;

        [SerializeField] private AimObstacle cliff;
        [SerializeField] private AimObstacle[] obstacles;

        [Header("Field")] [SerializeField] private int obstacleCount;

        private readonly List<AimObstacle> obstaclesToCheck = new();

        public void SetUpField()
        {
            var obstacleIndices = new List<int>(Enumerable.Range(0, obstacles.Length));
            while (obstacleIndices.Count > obstacleCount)
                obstacleIndices.RemoveAt(Random.Range(0, obstacleIndices.Count));

            obstaclesToCheck.Clear();

            for (var i = 0; i < obstacles.Length; i++)
            {
                var obstacle = obstacles[i];
                obstacle.SetReticule(reticule);
                obstacle.gameObject.SetActive(obstacleIndices.Contains(i));

                obstaclesToCheck.Add(obstacle);
            }
            
            cliff.SetReticule(reticule);
            obstaclesToCheck.Add(cliff);
        }

        public bool ReticuleOverlapsObstacle()
        {
            return obstaclesToCheck.Any(ob => ob.ReticuleWithinBounds);
        }
    }
}