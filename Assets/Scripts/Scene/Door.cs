using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script on doors to make them open and get player color.

public class Door : MonoBehaviour
{
    
    [SerializeField] private MeshRenderer[] renderersToChange;
    [SerializeField] private FinishGame finishGame;
    public bool openAtStart;
    public bool lastDoor;
    private Animator animator;
    private LayerMask blockMask;
    private RaycastHit hit;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        blockMask = LayerMask.GetMask("Block");

        if (openAtStart)
        {
            openDoor();
        }
    }

    private void LateUpdate()
    {
        //Debug.DrawRay(transform.position + new Vector3(0f, 0.3f, -0.1f), Vector3.down * 1f, Color.green, 1f);

        if (Physics.Raycast(transform.position + new Vector3(0f, 0.3f, -0.1f), Vector3.down, out hit, 1f, blockMask))
        {
            if (hit.collider.gameObject.tag.Contains("Player"))
            {
                openDoor();

                var materialToSet = hit.collider.gameObject.GetComponent<MeshRenderer>().material;

                for (int i = 0; i < renderersToChange.Length; i++)
                {
                    renderersToChange[i].material = materialToSet;
                }

                if (lastDoor)
                {
                    //finishGame.EndGame();
                }
            }
        }
    }


    public void openDoor()
    {
        animator.SetTrigger("isOpen");
        this.enabled = false;
    }
}
