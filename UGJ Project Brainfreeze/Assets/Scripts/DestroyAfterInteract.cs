using System;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class DestroyAfterInteract : MonoBehaviour
{
    public bool RequireSuccessfulInteraction = true;
    public float DestroyAfterInteractionTimeInSeconds = 0;

    private IInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<IInteractable>();

        if (interactable == null)
        {
            throw new Exception($"No {nameof(IInteractable)} found on {gameObject.name}");
        }

        interactable.OnInteractedWith += HandleOnInteractedWith;
    }

    private void OnDestroy()
    {
        interactable.OnInteractedWith -= HandleOnInteractedWith;
    }

    private void HandleOnInteractedWith(object source, OnInteractedWithArgs eventArgs)
    {
        if (RequireSuccessfulInteraction && !eventArgs.InteractionSuccessful)
        {
            return;
        }

        Destroy(gameObject, DestroyAfterInteractionTimeInSeconds);
    }
}
