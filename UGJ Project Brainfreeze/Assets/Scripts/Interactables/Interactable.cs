using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour, IInteractable
{
    public event EventHandler<OnInteractedWithArgs> OnInteractedWith;
    public event EventHandler<OnEnterInteractableRadiusArgs> OnEnterInteractableRadius;
    public event EventHandler<OnExitInteractableRadiusArgs> OnExitInteractableRadius;

    public float InteractionRadius { get => interactionRadius; }
    public bool HasBeenInteractedWith { get; private set; }
    public bool HasBeenSuccessfullyInteractedWith { get; private set; }
    public bool HasBeenUnsuccessfullyInteractedWith { get; private set; }

    [SerializeField]
    private float interactionRadius = 3f;

    private PlayerInteractableManager playerInteractableManager;

    private readonly List<IActionable> staticInteractionActions = new List<IActionable>();
    private readonly List<IInteractionRule> interactionRules = new List<IInteractionRule>();


    public void Interact(GameObject interacter)
    {
        HasBeenInteractedWith = true;
        staticInteractionActions.ForEach(action => action.PerformAction());

        if (interactionRules.Count == 0 || interactionRules.All(rule => rule.ApplyInteractionRule(interacter, gameObject)))
        {
            HasBeenSuccessfullyInteractedWith = true;
            OnInteractedWith?.Invoke(gameObject, new OnInteractedWithArgs
            {
                TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                InteracterGameObject = interacter,
                InteractedWithGameObject = gameObject,
                InteractedWith = this,
                InteractionSuccessful = true
            });
        }
        else
        {
            HasBeenUnsuccessfullyInteractedWith = true;
            OnInteractedWith?.Invoke(gameObject, new OnInteractedWithArgs
            {
                TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                InteracterGameObject = interacter,
                InteractedWithGameObject = gameObject,
                InteractedWith = this,
                InteractionSuccessful = false
            });
        }
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

    protected virtual void Awake()
    {
        var attachedCollider = gameObject.AddComponent<SphereCollider>();
        attachedCollider.isTrigger = true;
        attachedCollider.radius = InteractionRadius;

        foreach (var action in GetComponentsInChildren<IActionable>())
        {
            staticInteractionActions.Add(action);
        }

        foreach (var rule in GetComponentsInChildren<IInteractionRule>())
        {
            interactionRules.Add(rule);
        }
    }

    protected virtual void Start()
    {
        playerInteractableManager = PlayerInteractableManager.Instance;
        playerInteractableManager.RegisterInteractable(this);
    }

    protected virtual void OnDestroy()
    {
        playerInteractableManager.DeregisterInteractable(this);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractionRadius);
    }
}
