using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class HelloWorldAction : MonoBehaviour, IActionable
{
    private Interactable interactable;

    public void PerformAction()
    {
        Debug.Log("Test from static action");
    }

    private void Awake()
    {
        interactable = GetComponent<Interactable>();

        if (interactable == null)
        {
            throw new ArgumentNullException(nameof(Interactable));
        }

        interactable.OnEnterInteractableRadius += HandleOnEnterInteractableRadius;
        interactable.OnExitInteractableRadius += HandleOnExitInteractableRadius;
        interactable.OnInteracted += HandleOnInteracted;
    }

    private void OnDestroy()
    {
        interactable.OnEnterInteractableRadius -= HandleOnEnterInteractableRadius;
        interactable.OnExitInteractableRadius -= HandleOnExitInteractableRadius;
        interactable.OnInteracted -= HandleOnInteracted;
    }

    private void HandleOnEnterInteractableRadius(object source, OnEnterInteractableRadiusArgs eventArgs)
    {
        Debug.Log("Test from handle on enter interactable");
    }

    private void HandleOnExitInteractableRadius(object source, OnExitInteractableRadiusArgs eventArgs)
    {
        Debug.Log("Test from handle on exit interactable");
    }

    private void HandleOnInteracted(object source, OnInteractArgs eventArgs)
    {
        Debug.Log("Test from handle on interact");
    }
}
