using UnityEngine;

public class TreeKing : MonoBehaviour
{
    [SerializeField] private float health = 200;

    [SerializeField] private PlayerManager playerManager;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            playerManager.OnTreeKingDied();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Adventurer"))
        {
            Adventurer adventurer = other.gameObject.GetComponent<Adventurer>();
            adventurer.TakeDamage(100f);
        }
    }
}
