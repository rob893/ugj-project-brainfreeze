using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Interactable[] Interactables;

    private Queue<Interactable> interactableQueue;

    private GameManager() { }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        interactableQueue = new Queue<Interactable>(Interactables);
    }
}
