using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public event EventHandler<OnInteractArgs> OnInteracted;
    public event EventHandler<OnEnterInteractableRadiusArgs> OnEnterInteractableRadius;
    public event EventHandler<OnExitInteractableRadiusArgs> OnExitInteractableRadius;

    public float InteractionRadius { get => interactionRadius; }

    [SerializeField]
    private float interactionRadius = 3f;

    private readonly List<IActionable> staticInteractionActions = new List<IActionable>();
    

    public void Interact(GameObject interacter)
    {
        staticInteractionActions.ForEach(action => action.PerformAction());
        
        OnInteracted?.Invoke(gameObject, new OnInteractArgs
        {
            TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            InteracterGameObject = interacter,
            InteractedWithGameObject = gameObject,
            InteractedWith = this
        });
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        OnEnterInteractableRadius?.Invoke(gameObject, new OnEnterInteractableRadiusArgs
        {
            TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            InteracterGameObject = other.gameObject,
            InteractedWithGameObject = gameObject,
            InteractedWith = this
        });
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        OnExitInteractableRadius?.Invoke(gameObject, new OnExitInteractableRadiusArgs
        {
            TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            InteracterGameObject = other.gameObject,
            InteractedWithGameObject = gameObject,
            InteractedWith = this
        });
    }

    private void Awake()
    {
        var attachedCollider = gameObject.AddComponent<SphereCollider>();
        attachedCollider.isTrigger = true;
        attachedCollider.radius = InteractionRadius;

        foreach (var action in GetComponents<IActionable>())
        {
            staticInteractionActions.Add(action);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractionRadius);
    }
}
