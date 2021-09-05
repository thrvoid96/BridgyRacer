using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IPooledObject
{
    private Vector3 startPos;
    private float upForce = 10f;
    private float sideForce = 2f;
    private BoxCollider boxcol;
    private Rigidbody rb;
    private bool canSpawnAgain = true;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void onObjectSpawn()
    {        
        if (this.gameObject.tag.Contains("Grey"))
        {
            float xForce = Random.Range(-sideForce, sideForce);
            float yForce = Random.Range(upForce / 2f, upForce);
            float zForce = Random.Range(-sideForce, sideForce);

            Vector3 force = new Vector3(xForce, yForce, zForce);

            boxcol = GetComponent<BoxCollider>();
            rb = GetComponent<Rigidbody>();

            rb.velocity = force;
        }
        else
        {
            startPos = transform.position;
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

    public void playCollectAnimation()
    {
        animator.SetTrigger("isCollecting");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor") && this.gameObject.tag.Contains("Grey"))
        {
            boxcol.isTrigger = false;
            rb.velocity = new Vector3(0,5f,0);
        }
    }

}
