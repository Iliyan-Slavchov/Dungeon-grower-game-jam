using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Adventurer[] adventurersPrefab;
    [SerializeField] private Adventurer adventurerKingPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private RevealRoomManager revealRoomManager;
    [SerializeField] private TowerSpawningManager towerSpawningManager;
    [SerializeField] private TreeKingTeleportManager treeKingTeleportManager;
    [SerializeField] private TreeKingGrowingManager treeKingGrowingManager;
    [SerializeField] private TreeKing treeKing;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private AudioClip waveMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip waveClear;

    private int currentAdventurers = 0;
    private int spawnedAdventurers = 0;
    [SerializeField] private int maxAdventurers = 3;
    private bool isSpawned = false;
    private float timer;

    public bool isPlaying = true;
    public int currentWave = 9;
    private int kingGold;
    private bool kingSpawned = false;
    private Animator treeKingAnimator;
    private bool bossMusicStarted = false;

    private void Start()
    {
        treeKingAnimator = treeKing.GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentWave % 10 == 0 && !bossMusicStarted)
        {
            bossMusicStarted = true;
            musicAudioSource.Stop();
            musicAudioSource.clip = bossMusic;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

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

        if (currentWave % 10 == 0 && !kingSpawned && timer < 0f)
        {
            Adventurer adventurerKing = Instantiate(adventurerKingPrefab, spawnPosition.position, Quaternion.identity);
            adventurerKing.Died += OnAdventurerDied;
            currentAdventurers++;
            spawnedAdventurers++;
            maxAdventurers++;
            kingGold = 10;
            kingSpawned = true;
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
        StartCoroutine(WaveDefeatedRoutine());
    }

    private IEnumerator WaveDefeatedRoutine()
    {
        soundEffectAudioSource.PlayOneShot(waveClear);

        uiManager.UpdateText("Wave Defeated");
        revealRoomManager.RevealNextRoom();

        string animationName = "";

        switch (currentWave)
        {
            case 1:
                animationName = "Stage1Teleport";
                break;
            case 2:
                animationName = "Stage1Teleport";
                break;
            case 3:
                animationName = "Stage2Teleport";
                break;
            case 4:
                animationName = "Stage2Teleport";
                break;
            case 5:
                animationName = "Stage2Teleport";
                break;
            case 6:
                animationName = "Stage3Teleport";
                break;
            case 7:
                animationName = "Stage3Teleport";
                break;
            case 8:
                animationName = "Stage3Teleport";
                break;
        }

        timer = 10f;
        currentWave++;

        if (animationName != "")
        {
            treeKingAnimator.Play(animationName, 0, 0f);

            yield return null;

            yield return new WaitUntil(() =>
                treeKingAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            );

            yield return new WaitUntil(() =>
                treeKingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f
            );

            treeKingAnimator.speed = 0f;

            treeKingTeleportManager.TeleportTreeKingToNextRoom();

            treeKingGrowingManager.GrowTreeKing(currentWave);

            treeKingAnimator.speed = 1f;
        }

        isSpawned = false;

        towerSpawningManager.GiveGold(maxAdventurers + kingGold);

        kingGold = 0;
        maxAdventurers++;
        spawnedAdventurers = 0;

        timer = 10f;

        uiManager.UpdateCurrentWave(currentWave);

        kingSpawned = false;

        treeKingGrowingManager.GrowTreeKing(currentWave);

        bossMusicStarted = false;

        musicAudioSource.Stop();
        musicAudioSource.clip = waveMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }
}
