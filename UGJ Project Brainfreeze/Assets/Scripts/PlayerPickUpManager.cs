using System;
using UnityEngine;

public class PlayerPickUpManager : MonoBehaviour
{
    public bool HasItemPickedUp { get => ItemPickedUp != null; }
    public InteractableItem ItemPickedUp { get; private set; }

    [SerializeField]
    private KeyCode dropKey = KeyCode.R;
    [SerializeField]
    private Transform pickupTransform = null;

    private PlayerInteractableManager interactableManager;


    private void Start()
    {
        interactableManager = PlayerInteractableManager.Instance;

        if (pickupTransform == null)
        {
            throw new Exception("pickupTransform not set. Be sure to set it in the inspector.");
        }

        interactableManager.OnPlayerInteracted += HandleOnPlayerInteracted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(dropKey))
        {
            DropItem();
        }
    }

    private void OnDestroy()
    {
        interactableManager.OnPlayerInteracted -= HandleOnPlayerInteracted;
    }

    private void HandleOnPlayerInteracted(object source, OnInteractedWithArgs eventArgs)
    {
        var interactiableItem = eventArgs.InteractedWithGameObject.GetComponent<InteractableItem>();

        if (interactiableItem != null && interactiableItem.Item.CanPickUp)
        {
            ItemPickedUp = interactiableItem;
            ItemPickedUp.transform.parent = pickupTransform;
            ItemPickedUp.transform.localPosition = new Vector3(0, 0, 0);

            foreach (var collider in ItemPickedUp.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }
    }

    private void DropItem()
    {
        if (HasItemPickedUp)
        {
            foreach (var collider in ItemPickedUp.GetComponentsInChildren<Collider>())
            {
                collider.enabled = true;
            }

            ItemPickedUp.transform.parent = null;
            ItemPickedUp = null;
        }
    }
}
