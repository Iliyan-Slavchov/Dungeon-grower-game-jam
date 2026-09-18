using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSpawningManager : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;

    private PlayerActions playerActions;

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        playerActions.SpawnMenu.Enable();
    }

    private void Update()
    {
        if (playerActions.SpawnMenu.SpawnTower.WasPressedThisFrame())
        {
            Debug.Log("Spawn tower");
            Instantiate(towerPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, -Camera.main.transform.position.z)), Quaternion.identity);
        }
    }
}
