using UnityEngine;

public class TowerPatrolMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Vector2 patrolPosition1;
    private Vector2 patrolPosition2;

    private Vector2 target;

    [SerializeField] private float speed = 1f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        patrolPosition1 = GetGroundPosition(transform.position.x + 1f);
        patrolPosition2 = GetGroundPosition(transform.position.x - 1f);

        target = patrolPosition2;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            spriteRenderer.flipX = target.x > transform.position.x;
            target = target == patrolPosition1
                ? patrolPosition2
                : patrolPosition1;
        }
    }

    private Vector2 GetGroundPosition(float x)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(x, transform.position.y + 10f),
            Vector2.down,
            100f,
            groundLayer
        );

        return hit.point + Vector2.up * 0.3f;
    }
}