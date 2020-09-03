using UnityEngine;

public class RequireItemRule : MonoBehaviour, IInteractionRule
{
    public Item RequiredItem;

    public bool ApplyInteractionRule(GameObject interacter, GameObject interactee)
    {
        var pickUpManager = interacter.GetComponent<PlayerPickUpManager>();

        if (pickUpManager == null)
        {
            return false;
        }

        if (pickUpManager.ItemPickedUp.Item != RequiredItem)
        {
            return false;
        }

        return true;
    }
}
