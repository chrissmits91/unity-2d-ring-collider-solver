using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class RingColliderSolver2D : MonoBehaviour
{
    [SerializeField] private float ringThickness = 0.2f;
    [SerializeField] private float openingAngle = 45f;
    [SerializeField] private float angleIncrements = 1f;

    private PolygonCollider2D polygonCollider2D;
    private SpriteRenderer spriteRenderer;
    private Material material;

    private void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonCollider2D.points = new Vector2[]{};

        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;

        material.SetFloat("_Arc1", openingAngle / 2f);
        material.SetFloat("_Arc2", openingAngle / 2f);

        var halfOpeningAngle = openingAngle / 2f;
        var polygonColliderPoints = new List<Vector2>();
        var steps = (360f - openingAngle) / angleIncrements;
        for (var i = 0; i <= steps; i++) {
            var angle = halfOpeningAngle + i * angleIncrements;
            var dir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            polygonColliderPoints.Add((Vector2)transform.position + dir * (1f - ringThickness));
        }
        for (var i = steps; i >= 0; i--) {
            var angle = halfOpeningAngle + i * angleIncrements;
            var dir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            polygonColliderPoints.Add((Vector2)transform.position + dir);
        }

        polygonCollider2D.points = polygonColliderPoints.ToArray();
    }

    private void OnDrawGizmos() 
    {
        var halfAngle = openingAngle / 2f;
        var directionFromAngle = new Vector2(Mathf.Sin(halfAngle * Mathf.Deg2Rad), Mathf.Cos(halfAngle * Mathf.Deg2Rad));
        var startingPoint = (Vector2)transform.position + directionFromAngle * (1f - ringThickness);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, startingPoint);
    }
}
