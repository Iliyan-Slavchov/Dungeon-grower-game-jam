using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleStoryManager : MonoBehaviour
{

    [SerializeField] private Sprite[] storyImages;
    [SerializeField] private AudioClip[] voiceLines;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private WaveManager waveManager;

    private int index;
    private PlayerActions playerActions;
    private AudioSource voiceLineAudioSource;
    private bool hasPlayed = false;
    private bool storyStarted = false;

    private void Awake()
    {
        playerActions = new PlayerActions();

        voiceLineAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerActions.ContinueDialogue.Enable();
    }

    private void OnDisable()
    {
        playerActions.ContinueDialogue.Disable();
    }

    private void Update()
    {
        if (hasPlayed) { return; }

        if (waveManager.currentWave == 10 && !storyStarted) 
        {
            storyStarted = true;

            waveManager.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
            text.gameObject.SetActive(true);

            voiceLineAudioSource.Stop();
            image.sprite = storyImages[0];
            voiceLineAudioSource.clip = voiceLines[0];
            voiceLineAudioSource.Play();
        }

        if (playerActions.ContinueDialogue.Continue.WasPressedThisFrame() && waveManager.currentWave == 10)
        {
            index++;

            if (index >= storyImages.Length)
            {
                hasPlayed = true;

                image.gameObject.SetActive(false);
                text.gameObject.SetActive(false);

                waveManager.gameObject.SetActive(true);

                voiceLineAudioSource.Stop();

                return;
            }

            voiceLineAudioSource.Stop();
            image.sprite = storyImages[index];
            voiceLineAudioSource.clip = voiceLines[index];
            voiceLineAudioSource.Play();
        }
    }
}
