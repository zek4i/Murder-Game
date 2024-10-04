using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    int horizontal;
    int vertical;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal"); //now we are referencing the horizontal and vertical values of the parameter int the animator
        vertical = Animator.StringToHash("Vertical");
    }
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting, bool isWalking)
    {
        //animation snapping
        float snappedHorizontal; //optional to make animations seemless
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f; // Walking
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1f; // Running
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f; // Walking backward
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1f; // Running backward
        }
        else
        {
            snappedHorizontal = 0f; // Idle
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f; // Walking forward
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1f; // Running forward
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f; // Walking backward
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1f; // Running backward
        }
        else
        {
            snappedVertical = 0f; // Idle
        }
        #endregion

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement; // Maintain actual horizontal input
            snappedVertical = 2f; // Snap to sprint value
        }
        else if (isWalking)
        {
            snappedHorizontal = horizontalMovement; // Keep horizontal movement
            snappedVertical = 1f; // Snap to walk value
        }

        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);


    }
}
