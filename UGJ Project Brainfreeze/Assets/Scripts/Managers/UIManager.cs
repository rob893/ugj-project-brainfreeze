using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI sceneMessageText = null;
    [SerializeField]
    private TextMeshProUGUI interactToolTip = null;

    private PlayerInteractableManager playerInteractableManager;
    private float sceneMessageTimer = 0;

    private UIManager() { }

    private IEnumerator ShowMessageCoroutine(string message, float timeToShow, Color color)
    {
        sceneMessageText.text = message;
        sceneMessageText.color = color;
        sceneMessageText.gameObject.SetActive(true);

        while (sceneMessageTimer < timeToShow)
        {
            sceneMessageTimer += timeToShow;
            yield return new WaitForSeconds(timeToShow);
        }

        sceneMessageText.text = "";
        sceneMessageText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message, Color? color = null, float timeToShow = 3)
    {
        sceneMessageTimer = 0;

        if (sceneMessageText.IsActive())
        {
            sceneMessageText.text = message;
            sceneMessageText.color = color ?? Color.red;
            return;
        }

        StartCoroutine(ShowMessageCoroutine(message, timeToShow, color ?? Color.red));
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        sceneMessageText.enabled = false;
        interactToolTip.enabled = false;
    }

    private void Start()
    {
        playerInteractableManager = PlayerInteractableManager.Instance;
        playerInteractableManager.OnPlayerEnterInteractableRadius += HandleOnPlayerEnterInteractableRadius;
        playerInteractableManager.OnPlayerExitInteractableRadius += HandleOnPlayerExitInteractableRadius;
    }

    private void OnDestroy()
    {
        if (playerInteractableManager != null)
        {
            playerInteractableManager.OnPlayerEnterInteractableRadius -= HandleOnPlayerEnterInteractableRadius;
            playerInteractableManager.OnPlayerExitInteractableRadius -= HandleOnPlayerExitInteractableRadius;
        }
    }

    private void HandleOnPlayerEnterInteractableRadius(object source, OnEnterInteractableRadiusArgs eventArgs)
    {
        interactToolTip.text = $"Press {playerInteractableManager.InteractKey} to interact";
        interactToolTip.enabled = playerInteractableManager.PlayerInRangeOfInteractable; ;
    }

    private void HandleOnPlayerExitInteractableRadius(object source, OnExitInteractableRadiusArgs eventArgs)
    {
        interactToolTip.enabled = playerInteractableManager.PlayerInRangeOfInteractable;
    }
}
