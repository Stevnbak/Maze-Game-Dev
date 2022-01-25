using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float walkSpeed;
    public float startSpeed, sprintSpeed, crouchSpeed, acceleration, jumpPower, strafeMultiplier, gravity;
    float speed = 0, maxSpeed;
    
    [Header("Input")]
    public float sensitivity;
    public float deadzone, maxVerticalAngle;
    public Vector2 inputVec;
    public Vector2 mouseVec;

    [Header("Booleans")]
    public bool isSprinting = false;
    public bool isCrouching = false, isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        maxSpeed = walkSpeed;
        Physics.gravity = new Vector3(0,-gravity,0);
    }

    //Update
    void Update()
    {
        //Setting max Speed
        if (isSprinting) maxSpeed = sprintSpeed;
        if (isCrouching) maxSpeed = crouchSpeed;
        if (!isSprinting && !isCrouching) maxSpeed = walkSpeed;

        //Deadzone
        float x = mouseVec.x;
        float y = mouseVec.y;
        if (x < deadzone && x > -deadzone) x = 0;
        if (y < deadzone && y > -deadzone) y = 0;
        mouseVec = new Vector2(x, y);
    }

    //Fixed update
    void FixedUpdate()
    {
        //Speed calculation
        if (inputVec == new Vector2(0, 0))
        {
            GetComponent<Rigidbody>().velocity *= 0.9f;
            speed *= 0.9f;
            speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
        } else
        {
            if (speed < maxSpeed)
            {
                //Accelerate
                speed += acceleration * Time.fixedDeltaTime;
                speed = Mathf.Clamp(speed, startSpeed, maxSpeed);
            }
            if (speed > maxSpeed)
            {
                //Deaccelerate
                speed -= acceleration * Time.fixedDeltaTime;
                speed = Mathf.Clamp(speed, maxSpeed, 0);
            }
        }

        //Moving the player
        Movement();

        //Looking around
        Looking();
    }

    void Movement()
    {
        //Calculating direction velocity
        float horizontalRotation = transform.rotation.eulerAngles.y * Mathf.PI / 180;
        Vector2 yVector = new Vector2(Mathf.Sin(horizontalRotation) * inputVec.y, Mathf.Cos(horizontalRotation) * inputVec.y);
        Vector2 xVector = new Vector2(Mathf.Cos(horizontalRotation) * inputVec.x, -Mathf.Sin(horizontalRotation) * inputVec.x);
        Vector2 directionVec = xVector + yVector;

        //Move player
        Vector3 moveVector = new Vector3(directionVec.x * speed, GetComponent<Rigidbody>().velocity.y, directionVec.y * speed);
        GetComponent<Rigidbody>().velocity = moveVector;
        //Debug.Log("Movement Vector: " + moveVector);
    }

    void Looking()
    {
        //Horizontal look
        float xLook = mouseVec.x * 0.022f * sensitivity;
        //Debug.Log("Delta X: " + mouseVec.x + "Rotation X: " + xLook);
        transform.Rotate(0, xLook, 0);

        //Vertical look
        Transform lookingAt = transform.Find("LookingAt");
        float maxHeight = Mathf.Tan(maxVerticalAngle * Mathf.PI / 180) * lookingAt.localPosition.z;

        float yLook = mouseVec.y * 0.0011f * sensitivity;
        //Debug.Log("Delta Y: " + mouseVec.y + "Rotation Y: " + yLook);
        lookingAt.Translate(new Vector3(0, yLook * Mathf.Abs(lookingAt.position.z), 0));

        //Max height looking at can be
        
        //Debug.Log("Max looking at height: " + maxHeight);
        float height = Mathf.Clamp(lookingAt.localPosition.y, -maxHeight, maxHeight);
        lookingAt.localPosition = new Vector3(lookingAt.localPosition.x, height, lookingAt.localPosition.z);
    }

    public void Jump()
    {
        if (!isGrounded) return;
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    //IsGrounded?
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
