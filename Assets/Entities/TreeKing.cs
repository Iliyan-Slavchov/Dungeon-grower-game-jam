using UnityEngine;

public class TreeKing : MonoBehaviour
{
    [SerializeField] private float health = 200;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float damage = 100f;
    [SerializeField] private AudioClip deathAudioClip;
    private AudioSource audioSouce;

    private void Awake()
    {
        audioSouce = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            audioSouce.PlayOneShot(deathAudioClip);
            playerManager.OnTreeKingDied();
            Destroy(gameObject, 0.3f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Adventurer"))
        {
            Adventurer adventurer = other.gameObject.GetComponent<Adventurer>();
            adventurer.TakeDamage(damage);
        }
    }

}