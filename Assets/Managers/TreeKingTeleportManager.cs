using UnityEngine;

public class TreeKingTeleportManager : MonoBehaviour
{
    [SerializeField] private TreeKing treeKing;

    [SerializeField] private Transform[] roomTeleportPositions;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private int currentRoomIndex = 0;
    private int maxRoomIndex = 8;

    public void TeleportTreeKingToNextRoom()
    {
        if (currentRoomIndex < maxRoomIndex)
        {
            audioSource.PlayOneShot(audioClip);
            treeKing.transform.position = roomTeleportPositions[currentRoomIndex].position;
            currentRoomIndex++;
        }
    }
}
