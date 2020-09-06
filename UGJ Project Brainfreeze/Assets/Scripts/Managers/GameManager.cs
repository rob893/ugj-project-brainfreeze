using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public IInteractable NextInteractable { get => interactableQueue.Count > 0 ? interactableQueue.Peek() : null; }

    public Interactable[] Interactables;

    private Queue<IInteractable> interactableQueue;
    private PlayerInteractableManager playerInteractableManager;

    private GameManager() { }

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

        interactableQueue = new Queue<IInteractable>(Interactables);
    }

    private void Start()
    {
        playerInteractableManager = PlayerInteractableManager.Instance;
        playerInteractableManager.OnPlayerInteracted += HandleOnPlayerInteracted;
    }

    private void OnDestroy()
    {
        if (playerInteractableManager != null)
        {
            playerInteractableManager.OnPlayerInteracted -= HandleOnPlayerInteracted;
        }
    }

    private void HandleOnPlayerInteracted(object source, OnInteractedWithArgs eventArgs)
    {
        if (interactableQueue.Count == 0)
        {
            return;
        }

        var nextInteraction = interactableQueue.Peek();

        if (eventArgs.InteractionSuccessful && nextInteraction == eventArgs.InteractedWith)
        {
            interactableQueue.Dequeue();

            if (interactableQueue.Count == 0)
            {
                WinGame();
            }
        }
    }

    private void WinGame()
    {
        Debug.Log("You win!");
    }
}
