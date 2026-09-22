using UnityEngine;

public class TreeKingGrowingManager : MonoBehaviour
{
    [SerializeField] private TreeKing treeKing;
    [SerializeField] private BoxCollider2D treeKingCollider;


    private Animator treeKingAnimator;

    private void Start()
    {
        treeKingAnimator = treeKing.GetComponent<Animator>();
    }

    public void GrowTreeKing(int size)
    {
        switch (size)
        {
            case 1:
                treeKingAnimator.SetInteger("Stage", 1);
                break;
            case 3:
                treeKingAnimator.SetInteger("Stage", 2);
                break;
            case 6:
                treeKingAnimator.SetInteger("Stage", 3);
                break;
            case 9:
                treeKingAnimator.SetInteger("Stage", 4);
                break;
        }
    }
}
