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
        [SerializeField] private AimObstacle[] obstacles;

        [Header("Field")]
        [SerializeField] private int obstacleCount;

        private List<int> allObstacleIndices;
        private readonly List<AimObstacle> obstaclesToCheck = new();

        private void Awake()
        {
            allObstacleIndices = Enumerable.Range(0, obstacles.Length).ToList();

            foreach (var obstacle in obstacles)
                obstacle.SetReticule(reticule);
        }

        public void SetUpField()
        {
            var obstacleIndices = new List<int>(allObstacleIndices);
            while (obstacleIndices.Count > obstacleCount)
                obstacleIndices.RemoveAt(Random.Range(0, obstacleIndices.Count));
            
            obstaclesToCheck.Clear();

            for (var i = 0; i < obstacles.Length; i++)
            {
                var obstacle = obstacles[i];
                obstacle.gameObject.SetActive(obstacleIndices.Contains(i));
                
                obstaclesToCheck.Add(obstacle);
            }
        }

        public bool ReticuleOverlapsObstacle() => obstaclesToCheck.Any(ob => ob.ReticuleWithinBounds);
    }
}