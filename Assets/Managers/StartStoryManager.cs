using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private Sprite[] storyImages;
    [SerializeField] private AudioClip[] voiceLines;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private int index;
    private PlayerActions playerActions;
    private AudioSource voiceLineAudioSource;

    private void Awake()
    {
        playerActions = new PlayerActions();

        voiceLineAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
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
        if (playerActions.ContinueDialogue.Continue.WasPressedThisFrame())
        {
            text.gameObject.SetActive(true);

            index++;

            if (index >= storyImages.Length)
            {
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Single);

                return;
            }

            voiceLineAudioSource.Stop();
            image.sprite = storyImages[index];
            voiceLineAudioSource.clip = voiceLines[index];
            voiceLineAudioSource.Play();
        }
    }
}
