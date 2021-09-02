using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setGravity : MonoBehaviour
{
    public float gravityScale;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -gravityScale, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
