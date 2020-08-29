using System;
using UnityEngine;

public interface IInteractable
{
    event EventHandler<OnInteractArgs> OnInteracted;
    event EventHandler<OnEnterInteractableRadiusArgs> OnEnterInteractableRadius;
    event EventHandler<OnExitInteractableRadiusArgs> OnExitInteractableRadius;
    float InteractionRadius { get; }
    void Interact(GameObject interacter);
}
