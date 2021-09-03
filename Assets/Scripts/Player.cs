using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This position is put into the rig for shake effect, can click on inspector to see where it is.
    [SerializeField] private Transform stackStartPosition;

    private Vector3 addedPos;
    private bool isPlacing;
    private Stack<GameObject> blockStack = new Stack<GameObject>();
    private Animator animator;
    private float time;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        handleIdleTime();
        handleMovement();
        handleRotation();
    }

    //Rotate gameobject towards incoming input position
    private void handleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(animator.GetFloat("horizontal"), 0, animator.GetFloat("vertical"));

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
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

    
    private void OnTriggerEnter(Collider collider)
    {
        //Collect the block from the ground and add to the stack. Took me a while to perfect because using localpos and localrot didn't come to my mind first.
        if(collider.tag.Contains(this.gameObject.tag + "Block"))
        {
            if (isPlacing)
            {

            }

            var block = collider.gameObject;
            block.transform.parent = stackStartPosition;            
            block.transform.localPosition = addedPos;           
            block.transform.localRotation = Quaternion.Euler(0, 0, 0);
            addedPos += new Vector3(0, 0.1f, 0);

            blockStack.Push(block);           
        }
        else if (collider.CompareTag("Door"))
        {
            isPlacing = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (collider.CompareTag("Door"))
        {
            isPlacing = false;
        }
    }

}
