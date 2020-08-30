using System;
using UnityEngine;

[RequireComponent(typeof(IInteractable))]
public class PlayerInteractableRegistrar : MonoBehaviour
{
    private IInteractable interactable;
    private PlayerInteractableManager playerInteractableManager;


    private void Start()
    {
        interactable = GetComponent<IInteractable>();

        if (interactable == null)
        {
            throw new Exception($"No ${nameof(Interactable)} component found on GameObject {gameObject.name}");
        }

        playerInteractableManager = PlayerInteractableManager.Instance;
        playerInteractableManager.RegisterInteractable(interactable);
    }

    private void OnDestroy()
    {
        playerInteractableManager.DeregisterInteractable(interactable);
    }
}
