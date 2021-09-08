using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Behaviours
{
    public abstract class CommonBehaviours : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] protected Vector3 gapBetweenCollectedBlocks;
        [SerializeField] protected Transform stackStartTransform;
        [SerializeField] protected GameObject playerStopper;      
        [SerializeField] protected float knockbackForce = 3f;
        #endregion

        #region Components       
        protected Rigidbody rbody;
        protected Material playerMat;
        protected Animator animator;
        #endregion

        #region Variables
        protected Vector3 addedPos;
        protected bool isPlacing;
        protected bool isFalling;
        protected bool isGrounded;
        protected LayerMask groundMask;
        protected Stack<Block> blockStack = new Stack<Block>();
        #endregion


        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            playerMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
            rbody = GetComponent<Rigidbody>();
            groundMask = LayerMask.GetMask("Ground", "Block");
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            checkIfGrounded();
        }


        public int getBlockCountOnPlayer()
        {
            return blockStack.Count;
        }

        private void checkIfGrounded()
        {
            isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), Vector3.down, 1.5f, groundMask);
        }

        private void addBlockToStack(Block block)
        {
            block.disableAnimator();

            block.moveTowards(stackStartTransform, addedPos);

            addedPos += gapBetweenCollectedBlocks;

            blockStack.Push(block);
        }

        private void placeBlock(GameObject block)
        {
            if (blockStack.Count > 0)
            {
                block.GetComponent<MeshRenderer>().material = playerMat;
                block.tag = this.gameObject.tag + "Block";

                var poppedObj = blockStack.Pop();
                poppedObj.respawnCube(this.gameObject.tag + "Blocks");
                poppedObj.Inactivate();

                addedPos -= gapBetweenCollectedBlocks;

            }
            else
            {
                playerStopper.SetActive(true);
                playerStopper.transform.position = new Vector3(block.gameObject.transform.position.x, block.gameObject.transform.position.y, block.gameObject.transform.position.z - 0.1f);
            }
        }

        private void loseAllBlocks()
        {
            addedPos = new Vector3(0, 0, 0);
            var count = blockStack.Count;

            for (int i = 0; i < count; i++)
            {
                var block = blockStack.Pop();
                block.respawnCube(this.gameObject.tag + "Blocks");

                ObjectPooler.instance.SpawnFromPool("GreyBlocks", block.transform.position, block.transform.rotation, true);
                block.transform.parent = null;
                block.Inactivate();
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
                var block = other.GetComponent<Block>();

                if (collisionTag.Contains(this.gameObject.tag))
                {
                    if (isPlacing)
                    {
                        return;
                    }
                    addBlockToStack(block);
                }
                else if (collisionTag.Contains("Grey"))
                {
                    var obj = ObjectPooler.instance.SpawnFromPool(this.gameObject.tag + "Blocks", stackStartTransform.position + addedPos, stackStartTransform.rotation, false);
                    var spawnedBlock = obj.GetComponent<Block>();

                    addBlockToStack(spawnedBlock);
                    block.Inactivate();
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
            else if (collisionTag.Contains("Player"))
            {
                var enemyPlayer = other.gameObject.GetComponent<CommonBehaviours>();
                //Debug.LogError(enemyPlayer);

                if (enemyPlayer.blockStack.Count <= blockStack.Count)
                {
                    return;
                }


                isFalling = true;
                //animator.applyRootMotion = false;

                animator.SetTrigger("isFalling");
                animator.SetFloat("vertical", 0f);
                animator.SetFloat("horizontal", 0f);
                animator.SetFloat("idleTime", 0f);

                var collisionPoint = enemyPlayer.transform.position;
                transform.LookAt(new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z));
                rbody.AddExplosionForce(knockbackForce, new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z), 5f);

                loseAllBlocks();

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


        private void OnCollisionExit(Collision collision)
        {
            var tag = collision.gameObject.tag;
            if (tag.Contains("Floor") && !isGrounded)
            {
                isFalling = true;
                //animator.applyRootMotion = false;

                animator.SetTrigger("isFalling");
                animator.SetFloat("vertical", 0f);
                animator.SetFloat("horizontal", 0f);
                animator.SetFloat("idleTime", 0f);

                loseAllBlocks();
            }
        }

        public void setFallingFalse()
        {
            isFalling = false;
            //animator.applyRootMotion = true;
            animator.SetTrigger("fallingComplete");
        }
    }
}
