using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private BlockSpawner blockSpawner;
    [SerializeField] private Door[] sideDoors;
    [SerializeField] private Door[] oppositeDoors;

    public int gridToSpawn;
    public bool canSpawnGrid;
    public bool openAtStart;
    public bool gridAlreadySpawned;
    private Animator animator;
    private Door door;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        door = GetComponent<Door>();

        if (openAtStart)
        {
            openDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {            
            if (!openAtStart)
            {
                if (!gridAlreadySpawned && canSpawnGrid)
                {
                    blockSpawner.spawnSelectedGrid(gridToSpawn);

                    for (int i = 0; i < sideDoors.Length; i++)
                    {
                        sideDoors[i].gridAlreadySpawned = true;
                    }

                    for (int i = 0; i < oppositeDoors.Length; i++)
                    {
                        oppositeDoors[i].openDoor();
                    }
                }
            }

            openDoor();
            door.enabled = false;
        }
    }


    public void openDoor()
    {
        animator.SetTrigger("isOpen");
    }
}
