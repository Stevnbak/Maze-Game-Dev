using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractionInteract : MonoBehaviour, IInteractable
{
    public float time { get; set; }
    public Material lightMaterial;
    IGameController gameController;
    public bool hovering { get; set; }
    public GameObject inputPopup;
    Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        time = 1f;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>();
        lightMaterial.color = Color.red;
    }

    void Update()
    {
        inputPopup.transform.LookAt(player.position);

        if (hovering)
            inputPopup.SetActive(true);
        else inputPopup.SetActive(false);

        if (gameController.extractionReady)
        {
            lightMaterial.color = Color.green;
        } else
        {
            lightMaterial.color = Color.red;
        }
    }

    public void interact()
    {
        if (gameController.extractionReady) {
            PlayerPrefs.SetString("state", "win");
            gameController.EndGame(); 
        }
    }
}
