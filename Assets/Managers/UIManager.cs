using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameplayState;
    [SerializeField] private TMP_Text currentGold;
    [SerializeField] private TMP_Text insufficientGold;
    [SerializeField] private Button restartButton;

    private bool isEnabled = true;

    private void Start()
    {
        restartButton.gameObject.SetActive(false);
    }

    public void UpdateText(string text)
    {
        if (!isEnabled) { return; }

        gameplayState.enabled = true;
        gameplayState.text = text;
    }

    public void UpdateCurrentGold(int newCurrentGold)
    {
        currentGold.text = newCurrentGold.ToString();
    }

    public void ToggleInsufficientGold(bool Enable)
    {
        insufficientGold.enabled = Enable;
    }

    public void DisableText()
    {
        gameplayState.enabled = false;
    }

    public void ToggleTextUpdates(bool Enable)
    {
        isEnabled = Enable;
    }

    public void ToggleRestartButton(bool Enable)
    {
        restartButton.gameObject.SetActive(Enable);
    }
}
