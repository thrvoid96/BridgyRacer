using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IPooledObject
{    
    [SerializeField] private float bounceUpForce = 10f;
    [SerializeField] private float bounceSideForce = 2f;
    [SerializeField] private float speed = 0.3f;
    private Vector3 startPos;

    private BoxCollider boxcol;
    private Rigidbody rb;
    private Animator animator;
    private bool canSpawnAgain = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }
    public void onObjectSpawn()
    {
        StopAllCoroutines();

        startPos = transform.position;     

        boxcol.enabled = true;
        
        if (this.gameObject.tag.Contains("Grey"))
        {
            float xForce = Random.Range(-bounceSideForce, bounceSideForce);
            float yForce = Random.Range(bounceUpForce / 2f, bounceUpForce);
            float zForce = Random.Range(-bounceSideForce, bounceSideForce);

            Vector3 force = new Vector3(xForce, yForce, zForce);
            
            rb.velocity = force;
        }

    }

    public void setIfCanSpawn(bool value)
    {
        canSpawnAgain = value;
    }

    public void respawnCube(string cubeTag)
    {
        if (canSpawnAgain)
        {
            ObjectPooler.instance.SpawnFromPool(cubeTag, startPos, Quaternion.identity, true);
        }
    }

    public void disableAnimator()
    {
        if (animator != null)
        {
            animator.enabled = false;
        }

    }

    public void moveTowards(Transform stackTransform, Vector3 addedPosition)
    {
        boxcol.enabled = false;
        StartCoroutine(moveTowardsDestination(stackTransform,addedPosition));
    }

    public void Inactivate()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor") && this.gameObject.tag.Contains("Grey"))
        {
            boxcol.isTrigger = false;
            rb.velocity = new Vector3(0, 5f, 0);
        }
    }

    private IEnumerator moveTowardsDestination(Transform stackTransform, Vector3 addedPosition)
    {
        while (true)
        {
            var FinalDestination = stackTransform.position + addedPosition;
            var distance = Vector3.Distance(transform.position, FinalDestination);


            if (distance >= 0.1f)
            {
                // Rotate in arc
                SmoothLookAt(FinalDestination);

                // Move               
                //transform.LookAt(FinalDestination, Vector3.up);
                //transform.Translate (speed * transform.forward);
                
                transform.position = Vector3.MoveTowards(transform.position, stackTransform.position + addedPosition, speed);
                yield return null;
            }
            else
            {
                transform.parent = stackTransform;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = addedPosition;
                yield break;
            }
            
        }
    }

    private void SmoothLookAt(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        //Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
    
}
