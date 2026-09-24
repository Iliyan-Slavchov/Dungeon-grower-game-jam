using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    public void OnTreeKingDied()
    {
        audioSource.PlayOneShot(audioClip);
        waveManager.isPlaying = false;
        uiManager.UpdateText("Player Defeated");
        uiManager.ToggleTextUpdates(false);
        uiManager.ToggleRestartButton(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
