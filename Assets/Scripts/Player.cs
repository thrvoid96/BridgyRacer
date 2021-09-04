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
    private bool isFalling;
    private bool isGrounded;
    private Stack<GameObject> blockStack = new Stack<GameObject>();
    private Animator animator;
    private Rigidbody rbody;
    private float time;
    private LayerMask groundMask;

    public float knockbackForce = 3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        rbody = GetComponent<Rigidbody>();

        groundMask = LayerMask.GetMask("Ground","Block");
    }

    void Update()
    {
        checkIfGrounded();

        if (!isFalling && isGrounded)
        {
            HandleIdleTime();
            HandleMovement();
            HandleRotation();
        }     
    }

    private void checkIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, groundMask);
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

    private void addBlockToStack(GameObject blockGameobj)
    {
        blockGameobj.transform.parent = stackStartPosition;
        blockGameobj.transform.localPosition = addedPos;
        blockGameobj.transform.localRotation = Quaternion.Euler(0, 0, 0);

        addedPos += new Vector3(0, 0.1f, 0);

        blockStack.Push(blockGameobj);
    }

    private void placeBlock(GameObject blockGameobj)
    {
        if (blockStack.Count > 0)
        {
            blockGameobj.GetComponent<MeshRenderer>().material = playerMat;
            blockGameobj.tag = this.gameObject.tag + "Block";

            var poppedObj = blockStack.Pop();
            poppedObj.GetComponent<Block>().respawnCube(this.gameObject.tag + "Blocks");
            poppedObj.SetActive(false);
            addedPos -= new Vector3(0, 0.1f, 0);

        }
        else
        {
            //Move playerstopper into position
            playerStopper.SetActive(true);
            playerStopper.transform.position = new Vector3(blockGameobj.transform.position.x, blockGameobj.transform.position.y, blockGameobj.transform.position.z - 0.1f);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        var collisionTag = other.tag;

        if (other.CompareTag("PlacementArea"))
        {
            isPlacing = true;
            playerStopper.SetActive(false);

        }
        else if (collisionTag.Contains("Block") && !isFalling)
        {
             if (collisionTag.Contains(this.gameObject.tag))
            {
                if (isPlacing)
                {
                    return;
                }

                addBlockToStack(other.gameObject);
            }
            else 
            {

                if (!isPlacing)
                {
                    return;
                }

                placeBlock(other.gameObject);
            }

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

    private void OnCollisionEnter(Collision collision)
    {
        var collisionTag = collision.gameObject.tag;

        if (collisionTag.Contains("Player"))
        {
            /*var enemyPlayer = collision.gameObject.GetComponent<Player>();
            if(enemyPlayer.blockStack.Count <= blockStack.Count)
            {
                return;
            }
            */

            isFalling = true;
            animator.applyRootMotion = false;
            

            animator.SetTrigger("isFalling");
            animator.SetFloat("vertical", 0f);
            animator.SetFloat("horizontal", 0f);
            animator.SetFloat("idleTime", 0f);

            var collisionPoint = collision.GetContact(0).point;
            transform.LookAt(new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z));
            rbody.AddExplosionForce(knockbackForce, new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z), 5f);

            addedPos = new Vector3(0, 0, 0);
            var count = blockStack.Count;

            for(int i=0; i < count; i++)
            {
                var block = blockStack.Pop();
                block.GetComponent<Block>().respawnCube(this.gameObject.tag + "Blocks");

                ObjectPooler.instance.SpawnFromPool("GreyBlocks", block.transform.position, block.transform.rotation, true);
                block.transform.parent = null;
                block.SetActive(false);
            }
        }

        else if (collisionTag.Contains("Grey") && !isFalling)
            {
                var obj = ObjectPooler.instance.SpawnFromPool(this.gameObject.tag + "Blocks", stackStartPosition.position + addedPos, stackStartPosition.rotation, false);
                addBlockToStack(obj);
                collision.gameObject.SetActive(false);
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        var tag = collision.gameObject.tag;
        if (tag.Contains("Floor") && !isGrounded)
        {           
            isFalling = true;
            animator.applyRootMotion = false;

            animator.SetTrigger("isFalling");
            animator.SetFloat("vertical", 0f);
            animator.SetFloat("horizontal", 0f);
            animator.SetFloat("idleTime", 0f);
        }
    }


    public void setFallingFalse()
    {
        isFalling = false;
        animator.applyRootMotion = true;
        animator.SetTrigger("fallingComplete");
    }
}
