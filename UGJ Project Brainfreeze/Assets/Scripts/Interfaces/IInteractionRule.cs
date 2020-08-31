using UnityEngine;

public interface IInteractionRule
{
    bool ApplyInteractionRule(GameObject interacter, GameObject interactee);
}