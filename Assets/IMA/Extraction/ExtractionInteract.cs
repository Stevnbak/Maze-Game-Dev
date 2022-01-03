using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionInteract : MonoBehaviour, IInteractable
{
    public float time { get; set; }

    void Start()
    {
        time = 0.5f;
    }

    public void interact()
    {
        IGameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>();
        gameController.isGameRunning = false;
        //Switch to end game scene
    }
}
