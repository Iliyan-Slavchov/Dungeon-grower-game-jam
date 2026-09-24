using System;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public event Action<Adventurer> Died;

    [SerializeField] private float health = 100;
    [SerializeField] private float damage = 20f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] hitSounds;

    Rigidbody2D rigidbody2d;

    private int combatContacts = 0;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
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
        if (combatContacts <= 0)
        {
            rigidbody2d.AddForceX(100f, ForceMode2D.Impulse);
            rigidbody2d.linearVelocity = Vector2.ClampMagnitude(rigidbody2d.linearVelocity, 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            Tower tower = other.gameObject.GetComponent<Tower>();
            tower.TakeDamage(damage);
        }
        else if (other.CompareTag("TreeKing"))
        {
            TreeKing treeKing = other.gameObject.GetComponent<TreeKing>();
            treeKing.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tower") || other.CompareTag("TreeKing"))
        {
            int randomIndex = UnityEngine.Random.Range(0, hitSounds.Length);

            audioSource.PlayOneShot(hitSounds[randomIndex]);

            combatContacts++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tower") || other.CompareTag("TreeKing"))
            combatContacts--;
    }
}
