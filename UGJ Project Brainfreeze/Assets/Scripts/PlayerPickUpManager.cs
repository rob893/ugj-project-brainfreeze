using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpManager : MonoBehaviour
{
    public bool HasItemPickedUp { get => itemPickedUp != null; }

    [SerializeField]
    private Transform pickupTransform = null;

    private PlayerInteractableManager interactableManager;
    private GameObject itemPickedUp = null;

    private void Start()
    {
        interactableManager = PlayerInteractableManager.Instance;

        if (pickupTransform == null)
        {
            throw new Exception("pickupTransform not set. Be sure to set it in the inspector.");
        }

        interactableManager.OnPlayerInteracted += HandleOnPlayerInteracted;
    }

    private void OnDestroy()
    {
        interactableManager.OnPlayerInteracted -= HandleOnPlayerInteracted;
    }

    private void HandleOnPlayerInteracted(object source, OnInteractedWithArgs eventArgs)
    {
        if (itemPickedUp != null)
        {
            itemPickedUp.transform.parent = null;
            itemPickedUp = null;
        }
        else
        {
            eventArgs.InteractedWithGameObject.transform.parent = pickupTransform;
            itemPickedUp = eventArgs.InteractedWithGameObject;
        }
    }
}
