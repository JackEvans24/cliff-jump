using System.Collections.Generic;
using UnityEngine;

namespace CliffJump.UI
{
    [RequireComponent(typeof(Collider2D))]
    public class AimObstacle : MonoBehaviour
    {
        public bool ReticuleWithinBounds { get; private set; }
        public ObstacleType ObstacleType => obstacleType;

        [SerializeField] private ObstacleType obstacleType;
        [SerializeField] private ContactFilter2D contactFilter;

        private Collider2D coll;
        private AimReticule reticule;

        private readonly List<Collider2D> overlaps = new();

        public void SetReticule(AimReticule aimReticule) => reticule = aimReticule;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

        private void OnDisable()
        {
            ReticuleWithinBounds = false;
        }

        private void Update()
        {
            if (reticule == null)
                return;

            var overlapCount = coll.OverlapCollider(contactFilter, overlaps);
            ReticuleWithinBounds = overlapCount > 0 && overlaps.Contains(reticule.Collider);
        }
    }

    public enum ObstacleType
    {
        None,
        Rock,
        Cliff,
        Boat,
        Tilt
    }
}