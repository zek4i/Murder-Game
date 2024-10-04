using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public bool isSprinting;
    public bool isWalking;
    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput; // Movement input
        moveDirection += cameraObject.right * inputManager.horizontalInput; // Add horizontal input
        moveDirection.Normalize(); // Normalize to prevent faster diagonal movement
        moveDirection.y = 0; // Keep movement on the ground

        if(isWalking) // Check if walking
    {
            moveDirection *= walkingSpeed; // Use walking speed
        }
        else if (isSprinting)
        {
            moveDirection *= sprintingSpeed; // Use sprinting speed
        }
        else
        {
            // Check if the move amount is high enough to transition to running
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed; // Use running speed for high movement input
            }
            else
            {
                moveDirection *= walkingSpeed; // Default to walking speed
            }
        }


        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }
    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward; //when we stop moving keep the position of the player to the current position
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = playerRotation;
    }
}