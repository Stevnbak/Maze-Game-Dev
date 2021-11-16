using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float walkSpeed = 10;
    public float runSpeed = 15, crouchSpeed = 5, acceleration = 1, jumpPower = 5, gravity = 9.815f;
    float speed = 0, maxSpeed;
    
    [Header("Input")]
    public float sensitivity;

    //Other stuff
    CharacterController controller;

    void Start()
    { 
        controller = GetComponent<CharacterController>(); 
    }
 
    //Inputs
    public void MoveLeft()
    {
        Debug.Log("Fire!");
    }
    
    //Do the movement
    void FixedUpdate() {
        //Speed calculation
        if(speed <= maxSpeed) {
            speed += acceleration;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
        }
        if(speed < maxSpeed) {
            speed -= acceleration;
            speed = Mathf.Clamp(speed, maxSpeed, 0);
        }
        Debug.Log(speed);

        //Calculating direction velocity
        float horizontalRotation = transform.rotation.y;
        float zVelocity = Mathf.Cos(horizontalRotation * Mathf.PI / 180) * speed;
        float xVelocity = Mathf.Sin(horizontalRotation * Mathf.PI / 180) * speed;
        
        //Move character
        Vector3 moveVector = new Vector3(xVelocity, 0, zVelocity);
        Debug.Log(moveVector);
        controller.Move(moveVector);
    }
}
