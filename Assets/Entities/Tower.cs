using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
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
