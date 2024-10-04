using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAitTimer;
    public float leapingVelocity;
    public float fallingVelocity = 80f;
    public float rayCastHeightOffset = 0.1f; //starting raycast a little bit above the plane
    public LayerMask groundLayer;

    [Header ("Movemenent Flags")]
    public bool isSprinting;
    public bool isWalking;
    public bool isGrounded;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5;
    public float sprintingSpeed = 7;
    public float rotationSpeed = 15;
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void HandleAllMovement()
    {
        HandleFallingAndLanding(); //place it first because no matter what acting is being performed falling and landing should always be done
        if (playerManager.isInteracting) //the player will not perform other anim while its falling or landing 
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {

        // Prevent movement in the air
        if (!isGrounded)
        {
            return;
        }
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
        playerRigidbody.velocity = new Vector3(movementVelocity.x, playerRigidbody.velocity.y, movementVelocity.z); // Preserve y-velocity
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
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += rayCastHeightOffset;

        isGrounded = Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer);
        if (!isGrounded)
        {
            // Handle falling animation
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            // Apply forces for falling
            inAitTimer += Time.deltaTime;
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity);
        }
        else
        {
            // Handle landing animation only once
            if (!playerManager.isInteracting && inAitTimer > 0) // Ensuring there's some falling time before landing
            {
                animatorManager.PlayTargetAnimation("Landing", true);
                inAitTimer = 0; // Reset timer after landing

                playerRigidbody.MovePosition(hit.point);
            }
        }
    }
}