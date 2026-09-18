using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Adventurer adventurerPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private RevealRoomManager revealRoomManager;

    private int currentAdventurers = 0;
    private int spawnedAdventurers = 0;
    private int maxAdventurers = 3;
    private bool isSpawned = false;
    private float timer;

    private void Update()
    {
        SpawnAdventurer();
    }

    private void SpawnAdventurer()
    {
        timer -= Time.deltaTime;

        if (spawnedAdventurers < maxAdventurers && !isSpawned && timer < 0f)
        {
            uiManager.DisableText();
            Adventurer adventurer = Instantiate(adventurerPrefab, spawnPosition.position, Quaternion.identity);
            adventurer.Died += OnAdventurerDied;
            currentAdventurers++;
            spawnedAdventurers++;

            timer = 1f;

            if (currentAdventurers == maxAdventurers)
            {
                isSpawned = true;
            }
        }
    }

    private void OnAdventurerDied(Adventurer adventurer)
    {
        adventurer.Died -= OnAdventurerDied;

        currentAdventurers--;

        if (currentAdventurers <= 0 && spawnedAdventurers == maxAdventurers)
        {
            WaveDefeated();
        }
    }

    private void WaveDefeated()
    {
        uiManager.UpdateText("Wave Defeated");
        revealRoomManager.RevealNextRoom();
        isSpawned = false;
        maxAdventurers++;
        spawnedAdventurers = 0;
        timer = 10f;
    }
}
