using System;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PlayerInteractableRegistrar : MonoBehaviour
{
    private void Start()
    {
        var interactable = GetComponent<Interactable>();

        if (interactable == null)
        {
            throw new Exception($"No ${nameof(Interactable)} component found on GameObject {gameObject.name}");
        }

        PlayerInteractableManager.Instance.RegisterInteractable(interactable);
    }
}
