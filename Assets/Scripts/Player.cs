using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This position is put into the rig for shake effect, can click on inspector to see where it is.
    [SerializeField] private Transform stackStartPosition;
    [SerializeField] private GameObject playerStopper;

    private Material playerMat;
    private Vector3 addedPos;
    private bool isPlacing;
    private Stack<GameObject> blockStack = new Stack<GameObject>();
    private Animator animator;
    private float time;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    void Update()
    {
        HandleIdleTime();
        HandleMovement();
        HandleRotation();
    }

    //Rotate gameobject towards incoming input position
    private void HandleRotation()
    {
        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(animator.GetFloat("horizontal"), 0, animator.GetFloat("vertical"));

        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    //Basic movement
    private void HandleMovement()
    {
        animator.SetFloat("vertical", Input.GetAxis("Vertical"));
        animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));        
    }

    
    //When the player was turning 180 degrees, the velocity became 0 for a frame and the animator entered the idle state. Fix for that bug.
    private void HandleIdleTime()
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

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains(this.gameObject.tag + "Block") || other.gameObject.CompareTag("GreyBlock"))                     
        {           
            //cant collect grey blocks if still placing
            if (isPlacing)
            {
                return;
            }

            //Collect the block from the ground and add to the stack. Took me a while to perfect because using localpos and localrot didn't come to my mind first.
            var block = other.gameObject;

            block.transform.parent = stackStartPosition;
            block.transform.localPosition = addedPos;           
            block.transform.localRotation = Quaternion.Euler(0, 0, 0);
            addedPos += new Vector3(0, 0.1f, 0);

            blockStack.Push(block);           
        }

        //If collided with other players blocks and also in placing state, 
        else if (other.tag.Contains("Block"))
        {
            if (!isPlacing)
            {
                return;
            }
            var block = other.gameObject;

            if (blockStack.Count > 0)
            {
                
                block.GetComponent<MeshRenderer>().material = playerMat;
                block.tag = this.gameObject.tag + "Block";
                
                var poppedObj = blockStack.Pop();
                poppedObj.SetActive(false);
                addedPos -= new Vector3(0, 0.1f, 0);

            }
            else
            {
                //Move playerstopper into position
                playerStopper.SetActive(true);
                playerStopper.transform.position = new Vector3(block.transform.position.x,block.transform.position.y,block.transform.position.z - 0.1f);               
            }
            
        }
                
        else if (other.CompareTag("PlacementArea"))
        {
            isPlacing = true;
            playerStopper.SetActive(false);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlacementArea"))
        {
            isPlacing = false;
            playerStopper.SetActive(false);
        }
    }

}
