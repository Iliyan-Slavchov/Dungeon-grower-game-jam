using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSpawningManager : MonoBehaviour
{
    [SerializeField] private Tower greenGroundTowerPrefab;
    [SerializeField] private Tower flyingTowerPrefab;
    [SerializeField] private Tower mushroomTowerPrefab;
    [SerializeField] private Tower zombieTowerPrefab;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private int slimeTowerCost = 2;
    [SerializeField] private int flyingTowerCost = 2;
    [SerializeField] private int mushroomTowerCost = 2;
    [SerializeField] private int zombieTowerCost = 2;

    private PlayerActions playerActions;

    private int gold;

    private float timer;

    private Vector2 minPlacementBounds = new Vector2(-7, -4);
    private Vector2 maxPlacementBounds = new Vector2(7, 4);

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        playerActions.SpawnMenu.Enable();
        playerActions.Exit.Enable();
    }

    private void OnDisable()
    {
        playerActions.SpawnMenu.Disable();
        playerActions.Exit.Disable();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            uiManager.ToggleInsufficientGold(false);
        }

        if (playerActions.SpawnMenu.SpawnGreenGroundTower.WasPressedThisFrame() && gold >=slimeTowerCost && IsWithinBounds(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z))))
        {
            gold -= slimeTowerCost;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(greenGroundTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnGreenGroundTower.WasPressedThisFrame() && gold < slimeTowerCost)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnFlyingTower.WasPressedThisFrame() && gold >= flyingTowerCost && IsWithinBounds(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z))))
        {
            gold -= flyingTowerCost;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(flyingTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnFlyingTower.WasPressedThisFrame() && gold < flyingTowerCost)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnMushroomTower.WasPressedThisFrame() && gold >= mushroomTowerCost && IsWithinBounds(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z))))
        {
            gold -= mushroomTowerCost;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(mushroomTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnMushroomTower.WasPressedThisFrame() && gold < mushroomTowerCost)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.SpawnMenu.SpawnZombieTower.WasPressedThisFrame() && gold >= zombieTowerCost && IsWithinBounds(Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z))))
        {
            gold -= zombieTowerCost;
            uiManager.UpdateCurrentGold(gold);
            Instantiate(zombieTowerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
        else if (playerActions.SpawnMenu.SpawnZombieTower.WasPressedThisFrame() && gold < zombieTowerCost)
        {
            uiManager.ToggleInsufficientGold(true);
            timer = 5f;
        }

        if (playerActions.Exit.Exit.WasPressedThisFrame())
        {
            Application.Quit();
        }
    }

    public void GiveGold(int newGold)
    {
        gold += newGold;
        uiManager.UpdateCurrentGold(gold);
    }

    private bool IsWithinBounds(Vector2 position)
    {
        return position.x >= minPlacementBounds.x &&
               position.x <= maxPlacementBounds.x &&
               position.y >= minPlacementBounds.y &&
               position.y <= maxPlacementBounds.y;
    }
}
