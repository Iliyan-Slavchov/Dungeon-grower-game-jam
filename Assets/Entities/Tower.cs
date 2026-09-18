using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float health = 100;

    public event Action<Tower> Died;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();

        playerManager.TowerSpawned(this);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Adventurer"))
        {
            Adventurer adventurer = other.gameObject.GetComponent<Adventurer>();
            adventurer.TakeDamage(50f);
        }
    }
}
