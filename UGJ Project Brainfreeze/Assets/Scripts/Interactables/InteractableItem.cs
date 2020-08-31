using UnityEngine;

public class InteractableItem : Interactable
{
    public Item Item { get => item; }

    [SerializeField]
    private Item item = null;
}
