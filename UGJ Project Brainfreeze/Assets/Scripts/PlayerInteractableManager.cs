using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableManager : MonoBehaviour
{
    public static PlayerInteractableManager Instance { get; private set; }

    public bool PlayerInRangeOfInteractable { get; private set; }

    [SerializeField]
    private KeyCode interactKey = KeyCode.E;

    private GameObject player;

    private readonly List<IInteractable> interactablesPlayerIsInRangeOf = new List<IInteractable>();
    private readonly List<IInteractable> interactables = new List<IInteractable>();

    private PlayerInteractableManager() { }

    public void RegisterInteractable(IInteractable interactable)
    {
        interactables.Add(interactable);

        interactable.OnEnterInteractableRadius += HandleOnEnterInteractableRadius;
        interactable.OnExitInteractableRadius += HandleOnExitInteractableRadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            interactablesPlayerIsInRangeOf.ForEach(interactable => interactable.Interact(gameObject));
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag(Constants.PlayerTag);

        if (player == null)
        {
            throw new Exception($"Unable to find GameObject with {Constants.PlayerTag} tag.");
        }
    }

    private void OnDestroy()
    {
        foreach (var interactable in interactables)
        {
            interactable.OnEnterInteractableRadius -= HandleOnEnterInteractableRadius;
            interactable.OnExitInteractableRadius -= HandleOnExitInteractableRadius;
        }
    }

    private void HandleOnEnterInteractableRadius(object source, OnEnterInteractableRadiusArgs eventArgs)
    {
        if (!eventArgs.InteracterGameObject.CompareTag(Constants.PlayerTag))
        {
            return;
        }

        PlayerInRangeOfInteractable = true;
        interactablesPlayerIsInRangeOf.Add(eventArgs.InteractedWith);
    }

    private void HandleOnExitInteractableRadius(object source, OnExitInteractableRadiusArgs eventArgs)
    {
        if (!eventArgs.InteracterGameObject.CompareTag(Constants.PlayerTag))
        {
            return;
        }

        interactablesPlayerIsInRangeOf.Remove(eventArgs.InteractedWith);
        PlayerInRangeOfInteractable = interactablesPlayerIsInRangeOf.Count == 0;
    }
}
