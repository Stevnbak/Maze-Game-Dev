using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input_Manager : MonoBehaviour
{
    MovementController movementController;
    IWeapon weaponManager;

    private void Start()
    {
        movementController = GetComponent<MovementController>();
        weaponManager = GetComponentInChildren<IWeapon>();
        weaponManager.Initialize();
    }

    //Movement
    public void OnMove(InputValue input)
    {
        movementController.inputVec = input.Get<Vector2>();
        movementController.inputVec.x *= movementController.strafeMultiplier;
    }
    public void OnLook(InputValue input)
    {
        movementController.mouseVec = input.Get<Vector2>();
    }
    public void OnSprint(InputValue input)
    {
        if (input.Get<float>() == 0) movementController.isSprinting = false;
        if (input.Get<float>() == 1) movementController.isSprinting = true;
    }
    public void OnCrouch(InputValue input)
    {
        if (input.Get<float>() == 0) movementController.isCrouching = false;
        if (input.Get<float>() == 1) movementController.isCrouching = true;
    }
    public void OnJump(InputValue input)
    {
        if (input.Get<float>() == 1) movementController.Jump();
    }

    //Weapon
    void OnFire(InputValue input)
    {
        if (input.Get<float>() == 1)
        {
            weaponManager.Fire();
            weaponManager.isFiring = true;
            weaponManager.isReloading = false;
        }
        else
        {
            weaponManager.isFiring = false;
            weaponManager.fireTime = 0;
        }
    }

    void OnReload(InputValue input)
    {
        if (input.Get<float>() == 1)
        {
            weaponManager.isReloading = true;
            weaponManager.isADSing = false;
            weaponManager.isFiring = false;
        }
    }

    void OnADS(InputValue input)
    {
        if (input.Get<float>() == 1)
        {
            weaponManager.isReloading = false;
            weaponManager.isADSing = true;
        } else
        {
            weaponManager.isADSing = false;
        }
    }
}
