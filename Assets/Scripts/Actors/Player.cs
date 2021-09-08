using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

public class Player : CommonBehaviours
{
    [SerializeField] protected float rotationSpeed = 3f;
    private float time;   

    protected override void Update()
    {
        base.Update();

        if (!isFalling && isGrounded)
        {
            handleIdleTime();
            handleMovement();
            handleRotation();
        }     
    }  

    //Rotate gameobject towards incoming input position smoothly.
    private void handleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 positionToLookAt = currentPosition + newPosition;
        if (newPosition != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(positionToLookAt - currentPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

    }

    //Basic movement
    private void handleMovement()
    {
        animator.SetFloat("vertical", Input.GetAxis("Vertical"));
        animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));        
    }

    
    //When the player was turning 180 degrees, the velocity became 0 for a frame and the animator entered the idle state. Fix for that bug.
    private void handleIdleTime()
    {
        if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
        {
            time += Time.deltaTime;
            animator.SetFloat("idleTime", time);
            return;
        }
        else
        {
            time = 0f;
            animator.SetFloat("idleTime", time);
        }
    }


}
