using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        handleIdleTime();
        handleMovement();
        handleRotation();
    }

    private void handleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(animator.GetFloat("horizontal"), 0, animator.GetFloat("vertical"));

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    private void handleMovement()
    {
        animator.SetFloat("vertical", Input.GetAxis("Vertical"));
        animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));        
    }

    
    //Needed when player is moving by 1 button. Because it was entering the idle state in between.
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
