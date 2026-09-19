using UnityEngine;

public class TreeKingTeleportManager : MonoBehaviour
{
    [SerializeField] private TreeKing treeKing;

    [SerializeField] private Transform[] roomTeleportPositions;
    private int currentRoomIndex = 0;
    private int maxRoomIndex = 8;

    public void TeleportTreeKingToNextRoom()
    {
        if (currentRoomIndex < maxRoomIndex)
        {
            treeKing.transform.position = roomTeleportPositions[currentRoomIndex].position;
            currentRoomIndex++;
        }
    }
}
