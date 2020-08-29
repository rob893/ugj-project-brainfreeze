using UnityEngine;

public struct OnEnterInteractableRadiusArgs
{
    public long TimeStamp { get; set; }
    public GameObject InteracterGameObject { get; set; }
    public GameObject InteractedWithGameObject { get; set; }
    public IInteractable InteractedWith { get; set; }
}