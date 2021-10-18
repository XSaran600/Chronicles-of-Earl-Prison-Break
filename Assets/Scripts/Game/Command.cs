using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Saran Krishnaraja

// Base Class Command
public abstract class Command
{
    public abstract void Execute(float input_X, float input_Y, bool input_Sprint, float moveSpeed, Rigidbody rb, Transform trans, Animator mAnimator, AudioSource audioSource, AudioClip footSteps, Command command);
}

// Derived Class
public class Move : Command
{
    // Stats
    private float currentSpeed = 1f;
    private float minSpeed = 1f;
    private float maxSpeed = 2f;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 lookDir;

    public override void Execute(float input_X, float input_Y, bool input_Sprint,  float moveSpeed, Rigidbody rb, Transform trans, Animator mAnimator, AudioSource audioSource, AudioClip footSteps, Command command)
    {
        Movement(input_X, input_Y, input_Sprint, moveSpeed, rb, trans, mAnimator, audioSource, footSteps);
    }

    public void Movement(float input_X, float input_Y, bool input_Sprint, float moveSpeed, Rigidbody rb, Transform trans, Animator mAnimator, AudioSource audioSource, AudioClip footSteps)
    {
        if (input_Sprint && currentSpeed <= maxSpeed)        // If you're running and the currentSpeed is less than the maxSpeed
            currentSpeed = currentSpeed * 1.1f;             // Accelerate the character
        else if (!input_Sprint && currentSpeed > minSpeed)   // If you're not running and the currenSpeed is greater than the minSpeed
        {
            currentSpeed = currentSpeed * 0.1f;             // Decelerate the character
            if (currentSpeed < minSpeed)                    // If the currentSpeed is less than the minSpeed 
                currentSpeed = minSpeed;
        }

        // Get the input from joysticks and multiple it with the speeds
        moveInput = new Vector3(input_X, 0, input_Y);
        moveVelocity = moveInput * moveSpeed * currentSpeed;

        // Get the direction it should look at
        lookDir = (Vector3.right * input_X + Vector3.forward * input_Y) * -1;

        // Animations
        if (input_X != 0 || input_Y != 0)
            mAnimator.SetBool("IsWalking", true);
        else
            mAnimator.SetBool("IsWalking", false);

        mAnimator.SetFloat("Speed", currentSpeed);

        mAnimator.SetBool("Sprint", input_Sprint);

        //if (moveVelocity.x != 0 || moveVelocity.y != 0 || moveVelocity.z != 0)
            //audioSource.PlayOneShot(footSteps);   // Play footsteps sound

        // Move the character
        rb.velocity = moveVelocity;

        // Look Direction
        if (lookDir.sqrMagnitude > 0.0f)
            trans.rotation = Quaternion.LookRotation(lookDir);
    }
}