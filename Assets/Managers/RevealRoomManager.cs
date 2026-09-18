using UnityEngine;

public class RevealRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;

    private int currentRoomIndex = 0;
    private int maxRoomIndex = 8;

    public void RevealNextRoom()
    {
        if (currentRoomIndex < maxRoomIndex)
        {
            Destroy(gameObjects[currentRoomIndex]);
            currentRoomIndex++;
        }
    }
}
