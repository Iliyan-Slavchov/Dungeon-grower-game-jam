using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Adventurer[] adventurersPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private RevealRoomManager revealRoomManager;
    [SerializeField] private TowerSpawningManager towerSpawningManager;
    [SerializeField] private TreeKingTeleportManager treeKingTeleportManager;

    private int currentAdventurers = 0;
    private int spawnedAdventurers = 0;
    [SerializeField] private int maxAdventurers = 3;
    private bool isSpawned = false;
    private float timer;

    public bool isPlaying = true;

    private void Update()
    {
        SpawnAdventurer();
    }

    private void SpawnAdventurer()
    {
        if (!isPlaying) { return; }

        timer -= Time.deltaTime;

        if (spawnedAdventurers < maxAdventurers && !isSpawned && timer < 0f)
        {
            int prefabIndex = Random.Range(0, adventurersPrefab.Length);

            uiManager.DisableText();
            Adventurer adventurer = Instantiate(adventurersPrefab[prefabIndex], spawnPosition.position, Quaternion.identity);
            adventurer.Died += OnAdventurerDied;
            currentAdventurers++;
            spawnedAdventurers++;

            timer = 0.5f;

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
        treeKingTeleportManager.TeleportTreeKingToNextRoom();
        isSpawned = false;
        towerSpawningManager.GiveGold(maxAdventurers);
        maxAdventurers++;
        spawnedAdventurers = 0;
        timer = 10f;
    }
}
