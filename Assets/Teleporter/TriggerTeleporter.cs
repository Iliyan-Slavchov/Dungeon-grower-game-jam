using UnityEngine;

public class TriggerTeleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Adventurer"))
        {
            collision.gameObject.transform.position = destination.position;
        }
    }
}
