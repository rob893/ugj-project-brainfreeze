using UnityEngine;

public class RequireToBeNextInteractableRule : MonoBehaviour, IInteractionRule
{
    private GameManager gameManager;

    public bool ApplyInteractionRule(GameObject interacter, GameObject interactee)
    {
        var interactable = interactee.GetComponent<IInteractable>();

        if (interactable == null || interactable != gameManager.NextInteractable)
        {
            return false;
        }

        return true;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
