﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractableManager : MonoBehaviour
{
    public static PlayerInteractableManager Instance { get; private set; }

    public event EventHandler<OnInteractedWithArgs> OnPlayerInteracted;
    public event EventHandler<OnEnterInteractableRadiusArgs> OnPlayerEnterInteractableRadius;
    public event EventHandler<OnExitInteractableRadiusArgs> OnPlayerExitInteractableRadius;

    public bool PlayerInRangeOfInteractable { get => interactablesPlayerIsInRangeOf.Count > 0; }
    public KeyCode InteractKey { get => interactKey; }
    public MouseButton InteractMouseButton { get => interactMouseButton; }

    [SerializeField]
    private KeyCode interactKey = KeyCode.E;
    [SerializeField]
    private MouseButton interactMouseButton = MouseButton.Left;

    private GameObject player;
    private Camera playerCamera;

    private readonly HashSet<IInteractable> interactablesPlayerIsInRangeOf = new HashSet<IInteractable>();
    private readonly HashSet<IInteractable> interactables = new HashSet<IInteractable>();

    private PlayerInteractableManager() { }

    public void RegisterInteractable(IInteractable interactable)
    {
        if (interactables.Contains(interactable))
        {
            return;
        }

        interactables.Add(interactable);

        interactable.OnEnterInteractableRadius += HandleOnEnterInteractableRadius;
        interactable.OnExitInteractableRadius += HandleOnExitInteractableRadius;
        interactable.OnInteractedWith += HandleOnInteractedWith;
    }

    public void DeregisterInteractable(IInteractable interactable)
    {
        interactables.Remove(interactable);

        interactable.OnEnterInteractableRadius -= HandleOnEnterInteractableRadius;
        interactable.OnExitInteractableRadius -= HandleOnExitInteractableRadius;
        interactable.OnInteractedWith -= HandleOnInteractedWith;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown((int)interactMouseButton))
        {
            AttemptToInteract();
        }
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

        player = GameObject.FindGameObjectWithTag(Constants.PlayerTag);

        if (player == null)
        {
            throw new Exception($"Unable to find GameObject with {Constants.PlayerTag} tag.");
        }

        playerCamera = player.GetComponentInChildren<Camera>();

        if (playerCamera == null)
        {
            throw new Exception("Unable to find camera attached to player.");
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

    private void AttemptToInteract()
    {
        if (!PlayerInRangeOfInteractable)
        {
            return;
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hit, 100, ~(1 << 2), QueryTriggerInteraction.Ignore))
        {
            var hitInteractable = hit.transform.GetComponent<IInteractable>();

            if (hitInteractable != null && interactablesPlayerIsInRangeOf.Contains(hitInteractable))
            {
                Debug.Log($"Interacted with {hit.transform.name}");
                hitInteractable.Interact(player);
            }
        }
    }

    private void HandleOnEnterInteractableRadius(object source, OnEnterInteractableRadiusArgs eventArgs)
    {
        if (!eventArgs.InteracterGameObject.CompareTag(Constants.PlayerTag))
        {
            return;
        }

        interactablesPlayerIsInRangeOf.Add(eventArgs.InteractedWith);

        OnPlayerEnterInteractableRadius?.Invoke(gameObject, eventArgs);
    }

    private void HandleOnExitInteractableRadius(object source, OnExitInteractableRadiusArgs eventArgs)
    {
        if (!eventArgs.InteracterGameObject.CompareTag(Constants.PlayerTag))
        {
            return;
        }

        interactablesPlayerIsInRangeOf.Remove(eventArgs.InteractedWith);

        OnPlayerExitInteractableRadius?.Invoke(gameObject, eventArgs);
    }

    private void HandleOnInteractedWith(object source, OnInteractedWithArgs eventArgs)
    {
        if (!eventArgs.InteracterGameObject.CompareTag(Constants.PlayerTag))
        {
            return;
        }

        OnPlayerInteracted?.Invoke(gameObject, eventArgs);
    }
}
