using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Combine same behaviours between the Player and AI.

namespace Behaviours
{
    public abstract class CommonBehaviours : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] protected int playerNum;
        [SerializeField] protected Vector3 gapBetweenCollectedBlocks;
        [SerializeField] protected Transform stackStartTransform;
        [SerializeField] protected GameObject playerStopper;
        [SerializeField] protected float knockbackForce = 3f;       
        [SerializeField] protected SkinnedMeshRenderer playerSkinRenderer;

        #endregion

        #region Components       
        protected Rigidbody rbody;        
        protected Animator animator;
        private CapsuleCollider groundCollider;
        private BlockInteractions blockInteractions = new BlockInteractions();
        #endregion

        #region Variables
        public Vector3 nextBlockPosition
        {
            get { return addedPos; }
            set { addedPos = value; }
        }

        public Material getMaterial
        {
            get { return playerSkinRenderer.material; }
        }

        public int getPlayerNum
        {
            get { return playerNum; }
        }

        public GameObject lastBlockPlaced
        {
            get { return lastBlock; }
            set { lastBlock = value; }
        }

        public int blockStackCount
        {
            get { return blockStack.Count; }
        }

        protected GameObject lastBlock;
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
            rbody = GetComponent<Rigidbody>();
            groundCollider = GetComponentInChildren<CapsuleCollider>();
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
            if (other.CompareTag("PlacementArea"))
            {
                isPlacing = true;
                playerStopper.SetActive(false);
            }
            else if (other.tag.Contains("Block"))
            {
                if (!isFalling)
                {
                    var block = other.GetComponent<Block>();

                    if (block == null)
                    {
                        var stairBlock = other.GetComponent<StairBlock>();

                        if (stairBlock.blockNum != playerNum)
                        {
                            blockInteractions.placeBlock(this, stairBlock, gapBetweenCollectedBlocks, blockStack, playerStopper);
                        }
                    }
                    else if (other.tag.Contains("Grey"))
                    {
                        var obj = ObjectPooler.instance.SpawnFromPool("Player" + playerNum + "Blocks", stackStartTransform.position + addedPos, stackStartTransform.rotation, false);
                        var spawnedBlock = obj.GetComponent<Block>();

                        blockInteractions.addBlockToStack(this, spawnedBlock, stackStartTransform, gapBetweenCollectedBlocks, blockStack);
                        block.Inactivate();
                    }
                    else if (block.blockNum == playerNum)
                    {
                        blockInteractions.addBlockToStack(this, block, stackStartTransform, gapBetweenCollectedBlocks, blockStack);
                    }
                }

                else if(other.CompareTag("EmptyBlock"))
                {
                    groundCollider.enabled = false;
                }


            }
            else if (other.CompareTag("Player") && !isFalling)
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

            blockInteractions.loseAllBlocks(this, blockStack);
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
                blockInteractions.loseAllBlocks(this, blockStack);
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
