using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool openAtStart;
    private Animator animator;
    private Door door;
    private LayerMask blockMask;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        door = GetComponent<Door>();

        blockMask = LayerMask.GetMask("Block");

        if (openAtStart)
        {
            openDoor();
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0f, 0.3f, -0.1f), Vector3.down * 1f, Color.green, 1f);
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.3f, -0.1f), Vector3.down, out hit, 1f, blockMask))
        {
            if (hit.collider.gameObject.tag.Contains("Player"))
            {
                openDoor();
            }
        }
    }


    public void openDoor()
    {
        animator.SetTrigger("isOpen");
        door.enabled = false;
    }
}
