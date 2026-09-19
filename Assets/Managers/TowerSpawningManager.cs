using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSpawningManager : MonoBehaviour
{
    [SerializeField] private Tower greenGroundTowerPrefab;
    [SerializeField] private Tower flyingTowerPrefab;
    [SerializeField] private Tower mushroomTowerPrefab;
    [SerializeField] private Tower zombieTowerPrefab;
    [SerializeField] private UIManager uiManager;

    private PlayerActions playerActions;

    private int gold;

    private float timer;

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        playerActions.SpawnMenu.Enable();
    }

    private void OnDisable()
    {
        playerActions.SpawnMenu.Disable();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            uiManager.ToggleInsufficientGold(false);
        }

        if (playerActions.SpawnMenu.SpawnGreenGroundTower.WasPressedThisFrame() && gold >=2)
        {
            gold -= 2;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(greenGroundTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnGreenGroundTower.WasPressedThisFrame() && gold < 2)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnFlyingTower.WasPressedThisFrame() && gold >= 4)
        {
            gold -= 4;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(flyingTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnFlyingTower.WasPressedThisFrame() && gold < 4)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnMushroomTower.WasPressedThisFrame() && gold >= 3)
        {
            gold -= 3;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(mushroomTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnMushroomTower.WasPressedThisFrame() && gold < 3)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnZombieTower.WasPressedThisFrame() && gold >= 1)
        {
            gold -= 1;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(zombieTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnZombieTower.WasPressedThisFrame() && gold < 1)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }
    }

    public void GiveGold(int newGold)
    {
        gold += newGold;
        uiManager.UpdateCurrentGold(gold);
    }
}
