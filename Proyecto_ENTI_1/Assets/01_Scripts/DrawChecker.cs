using UnityEngine;

enum Shape
{
    Circle,
    Box
}
public class DrawChecker : MonoBehaviour
{
    [SerializeField] Shape shape = Shape.Circle;

    //Circle collider
    [SerializeField, Range(0.05f,2f)] float radius;
    [SerializeField] Vector2 circleCenter;

    //Box collider
    [SerializeField] Vector2 squareSize;
    [SerializeField] Vector2 squareCenter;
    [SerializeField] float angle;

    void Start()
    {
        
    }
    void Update ( )
    {

    }

    public Collider2D CheckColisions ( LayerMask layer )
    {
        if (shape == Shape.Circle)
        {
            Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position + circleCenter, radius, layer);
            return col;
        }
        else if (shape == Shape.Box)
        {
            Collider2D col = Physics2D.OverlapBox((Vector2)transform.position + squareCenter, squareSize, angle, layer);
            return col;
        }
        return null;
    }
    void OnDrawGizmosSelected ( )
    {
        if (shape == Shape.Circle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere((Vector2)transform.position + circleCenter, radius);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube((Vector2)transform.position + squareCenter, squareSize);
        }
    }
}
