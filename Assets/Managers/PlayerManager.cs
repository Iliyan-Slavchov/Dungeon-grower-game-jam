using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private int currentTowers;

    private void OnTowerDied(Tower tower)
    {
        tower.Died -= OnTowerDied;

        currentTowers--;

        if (currentTowers <= 0)
        {
            PlayerDefeated();
        }
    }

    private void PlayerDefeated()
    {
        uiManager.UpdateText("Player Defeated");
    }

    public void TowerSpawned(Tower tower)
    {
        Debug.Log("Tower Spawned");
        tower.Died += OnTowerDied;
        currentTowers++;
    }
}
