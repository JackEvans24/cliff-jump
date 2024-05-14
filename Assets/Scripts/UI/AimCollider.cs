using System.Collections.Generic;
using UnityEngine;

namespace CliffJump.UI
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class AimCollider : MonoBehaviour
    {
        [SerializeField] private AimReticule reticule;
        [SerializeField] private ContactFilter2D contactFilter;

        private Collider2D coll;
        private SpriteRenderer spriteRenderer;

        private readonly List<Collider2D> overlaps = new();
        private Color colour;

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            colour = Color.green;

            var overlapCount = coll.OverlapCollider(contactFilter, overlaps);
            if (overlapCount > 0 && overlaps.Contains(reticule.Collider))
                colour = Color.red;

            spriteRenderer.color = colour;
        }
    }
}