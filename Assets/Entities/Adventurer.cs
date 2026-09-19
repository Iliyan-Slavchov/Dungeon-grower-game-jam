using System;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public event Action<Adventurer> Died;

    [SerializeField] private float health = 100;

    Rigidbody2D rigidbody2d;

    private bool inCombat = false;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        Died?.Invoke(this);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!inCombat)
        {
            rigidbody2d.AddForceX(100f, ForceMode2D.Impulse);
            rigidbody2d.linearVelocity = Vector2.ClampMagnitude(rigidbody2d.linearVelocity, 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            inCombat = true;
            Tower tower = other.gameObject.GetComponent<Tower>();
            tower.TakeDamage(10f);
        }
        else if (other.CompareTag("TreeKing"))
        {
            inCombat = true;
            TreeKing treeKing = other.gameObject.GetComponent<TreeKing>();
            treeKing.TakeDamage(10f);
        }
    }
}
