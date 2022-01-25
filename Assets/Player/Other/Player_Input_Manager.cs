using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player_Input_Manager : MonoBehaviour
{
    MovementController movementController;
    IGameController gameController;
    IWeapon weaponManager;
    bool interacting;
    IInteractable interactable;
    Transform interactTrans;
    public float maxInteractDistance;
    float t;
    HUD hud;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IGameController>();
        hud = GameObject.Find("GameController").GetComponent<HUD>();
        movementController = GetComponent<MovementController>();
        weaponManager = GetComponentInChildren<IWeapon>();
        weaponManager.Initialize();
        Cursor.visible = false;
    }

    //Movement
    public void OnMove(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        movementController.inputVec = input.Get<Vector2>();
        movementController.inputVec.x *= movementController.strafeMultiplier;
    }
    public void OnLook(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        movementController.mouseVec = input.Get<Vector2>();
    }
    public void OnSprint(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 0) movementController.isSprinting = false;
        if (input.Get<float>() == 1) movementController.isSprinting = true;
    }
    public void OnCrouch(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 0) movementController.isCrouching = false;
        if (input.Get<float>() == 1) movementController.isCrouching = true;
    }
    public void OnJump(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1) movementController.Jump();
    }

    //Weapon
    void OnFire(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1)
        {
            weaponManager.Fire();
            weaponManager.isFiring = true;
            weaponManager.isReloading = false;
            interacting = false;
        }
        else
        {
            weaponManager.isFiring = false;
            weaponManager.fireTime = 0;
        }
    }

    void OnReload(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1)
        {
            weaponManager.isReloading = true;
            weaponManager.isADSing = false;
            weaponManager.isFiring = false;
            interacting = false;
        }
    }

    void OnADS(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1)
        {
            weaponManager.isReloading = false;
            weaponManager.isADSing = true;
            interacting = false;
        } else
        {
            weaponManager.isADSing = false;
        }
    }

    //Interact
    void OnInteraction(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1)
        {
            int layerMask = 1 << 6;
            layerMask = ~layerMask;
            Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, maxInteractDistance, layerMask))
            {
                Debug.DrawRay(Camera.main.transform.position, direction * hit.distance, Color.blue);
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.CompareTag("Item"))
                {
                    t = 0;
                    interactable = hitObject.GetComponent<IInteractable>();
                    interacting = true;
                    weaponManager.isReloading = false;
                    weaponManager.isFiring = false;
                    weaponManager.isADSing = false;
                    interactTrans = hitObject.transform;
                }
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().stopInteractTimer();
            t = 0;
            interacting = false;
        }
    }

    //Other
    void OnMap(InputValue input)
    {
        if (!gameController.isGameRunning) return;
        if (input.Get<float>() == 1)
        {
            GameObject.Find("Map").transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            GameObject.Find("Map").transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void OnShow_Objectives(InputValue input)
    {
        if (!gameController.isGameRunning) return;
            if (input.Get<float>() == 1)
        {
            hud.showingObjectiveScreen = true;
        }
        else
        {
            hud.showingObjectiveScreen = false;
        }
    }

    private void OnPause(InputValue input)
    {
        if (input.Get<float>() == 1)
        {
            GameObject.Find("GameController").GetComponent<PauseScreen>().Trigger();
        }
    }

    void Update()
    {
        if (interacting)
        {
            if (t == 0) GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().startInteractTimer(interactable.time);
            if (gameController.isGameRunning) t += Time.deltaTime;
            if (t >= interactable.time)
            {
                interactable.interact();
                t = 0;
                interacting = false;
            }
            if (Vector3.Distance(interactTrans.position, Camera.main.transform.position) > maxInteractDistance)
            {
                interacting = false;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<HUD>().stopInteractTimer();
                t = 0;
            }
        }

        if (!gameController.isGameRunning)
        {
            movementController.inputVec = Vector3.zero;
            movementController.mouseVec = Vector3.zero;
        }
    }
}



