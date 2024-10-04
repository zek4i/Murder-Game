using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool shift_input;
    public bool ctrl_input;
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>(); //moving w WASD
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            //all this means is that when u move the move it sends the input to the camera input vector 2 variable
            //we do the same and divide horizontal and vertical input likehow we did with player movement

            playerControls.PlayerAction.Shift.performed += i => shift_input = true;
            playerControls.PlayerAction.Shift.canceled += i => shift_input = false;

            playerControls.PlayerWalk.Walk.performed += i => ctrl_input = true; // Set walking to true
            playerControls.PlayerWalk.Walk.canceled += i => ctrl_input = false; // Set walking to false
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleWalkingInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); //abs makes always the value positive aka walking or running always needs a positive value

        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting, playerLocomotion.isWalking);

    }

    private void HandleSprintingInput()
    {
        if (shift_input && moveAmount > 0.5f) // Check if shift is pressed and player is moving
        {
            playerLocomotion.isSprinting = true; // Enable sprinting
            playerLocomotion.isWalking = false; // Disable walking when sprinting
        }
        else
        {
            playerLocomotion.isSprinting = false; // Disable sprinting
        }
    }

    private void HandleWalkingInput()
    {// Check if the control key (or your desired key) is pressed
        Debug.Log("Move Amount: " + moveAmount);
        if (ctrl_input && moveAmount > 0.1f) // If "Ctrl" is pressed while moving
        {
            playerLocomotion.isWalking = true; // Set the walking state to true
            playerLocomotion.isSprinting = false; // Disable sprinting when walking
        }
        else
        {
            playerLocomotion.isWalking = false; // Set the walking state to false
        }
    }
}