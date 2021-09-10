using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Combine same behaviours between Player and AI.

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
        private BlockInteractions blockInteractions = new BlockInteractions();
        #endregion

        #region Variables
        public Vector3 nextBlockPosition
        {
            get { return addedPos; }
            set { addedPos = value; }
        }

        public Vector3 lastBlockPlacedPosition
        {
            get { return lastBlockPos; }
            set { lastBlockPos = value; }
        }

        public int blockStackCount
        {
            get { return blockStack.Count; }
        }

        protected Vector3 lastBlockPos;
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
            groundMask = LayerMask.GetMask("Ground");
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            checkIfGrounded();
        }


        private void checkIfGrounded()
        {
            isGrounded = Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), Vector3.down, 1.5f, groundMask);
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
                    if (isPlacing || block == null)
                    {
                        return;
                    }
                    blockInteractions.addBlockToStack(this, block, stackStartTransform, gapBetweenCollectedBlocks, blockStack);
                }
                else if (collisionTag.Contains("Grey"))
                {
                    var obj = ObjectPooler.instance.SpawnFromPool(this.gameObject.tag + "Blocks", stackStartTransform.position + addedPos, stackStartTransform.rotation, false);
                    var spawnedBlock = obj.GetComponent<Block>();

                    blockInteractions.addBlockToStack(this, spawnedBlock, stackStartTransform, gapBetweenCollectedBlocks, blockStack);
                    block.Inactivate();
                }
                else
                {

                    if (!isPlacing)
                    {
                        return;
                    }

                    blockInteractions.placeBlock(this,other.gameObject,playerMat,gameObject.tag,gapBetweenCollectedBlocks,blockStack,playerStopper);
                }

            }
            else if (collisionTag.Contains("Player") && !isFalling)
            {
                var enemyPlayer = other.gameObject.GetComponent<CommonBehaviours>();

                if (enemyPlayer.blockStackCount <= blockStackCount)
                {
                    return;
                }

                setAnimatorFalling();

                getKnockedBack(enemyPlayer);

            }


        }

        private void getKnockedBack(CommonBehaviours enemyPlayer)
        {
            var collisionPoint = enemyPlayer.transform.position;
            transform.LookAt(new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z));
            rbody.AddExplosionForce(knockbackForce, new Vector3(collisionPoint.x, transform.position.y, collisionPoint.z), 5f);

            blockInteractions.loseAllBlocks(this, gameObject.tag, blockStack);
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
            if (tag.Contains("Floor") && !isGrounded && !isFalling)
            {
                setAnimatorFalling();
                blockInteractions.loseAllBlocks(this, gameObject.tag, blockStack);
            }
        }

        private void setAnimatorFalling()
        {
            isFalling = true;
            
            animator.SetTrigger("isFalling");
            animator.SetFloat("vertical", 0f);
            animator.SetFloat("horizontal", 0f);
            animator.SetFloat("idleTime", 0f);

            if (gameObject.GetComponent<Player>())
            {
                animator.applyRootMotion = false;
            }
        }

        public void setFallingFalse()
        {
            isFalling = false;
            animator.SetTrigger("fallingComplete");

            if (gameObject.GetComponent<Player>())
            {
                animator.applyRootMotion = true;
            }
        }
    }
}
