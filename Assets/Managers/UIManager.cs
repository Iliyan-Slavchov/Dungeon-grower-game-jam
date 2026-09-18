using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameplayState;

    public void UpdateText(string text)
    {
        gameplayState.enabled = true;
        gameplayState.text = text;
    }

    public void DisableText()
    {
        gameplayState.enabled = false;
    }
}
