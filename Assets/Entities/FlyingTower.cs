using UnityEngine;

public class FlyingTower : MonoBehaviour
{
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
        patrolPosition1 = new Vector2(transform.position.x + 1f, transform.position.y);
        patrolPosition2 = new Vector2(transform.position.x - 1f, transform.position.y);

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
            spriteRenderer.flipX = target.x < transform.position.x;
            target = target == patrolPosition1
                ? patrolPosition2
                : patrolPosition1;
        }
    }
}