using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairBlock : MonoBehaviour
{
    private Player player;
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }
}
