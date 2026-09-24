using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas1;
    [SerializeField] private Canvas canvas2;

    private PlayerActions playerActions;

    private bool isShowing1 = false;
    private bool isShowing2 = false;

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void Start()
    {
        canvas1.gameObject.SetActive(false);
        canvas2.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerActions.ShowCredits.Enable();
    }

    private void OnDisable()
    {
        playerActions.ShowCredits.Disable();
    }

    private void Update()
    {
        if (playerActions.ShowCredits.ShowCredits.WasPressedThisFrame())
        {
            canvas2.gameObject.SetActive(false);
            isShowing2 = false;

            if (isShowing1)
            {
                canvas1.gameObject.SetActive(false);
                isShowing1 = false;
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
            else if (!isShowing1)
            {
                canvas1.gameObject.SetActive(true);
                isShowing1 = true;
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
        }

        if (playerActions.ShowCredits.ShowControls.WasPressedThisFrame())
        {
            canvas1.gameObject.SetActive(false);
            isShowing1 = false;

            if (isShowing2)
            {
                canvas2.gameObject.SetActive(false);
                isShowing2 = false;
                Time.timeScale = 1f;
                AudioListener.pause = false;
            }
            else if (!isShowing2)
            {
                canvas2.gameObject.SetActive(true);
                isShowing2 = true;
                Time.timeScale = 0f;
                AudioListener.pause = true;
            }
        }
    }
}
