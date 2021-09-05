using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCam : MonoBehaviour
{
    [SerializeField] private GameObject playerToFollow;
    private Player player;
    private Camera cam;
    private int playerBlockCount;
    public Vector3 offsetValues;


    private void Start()
    {
       player = playerToFollow.GetComponent<Player>();
       cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.position = playerToFollow.transform.position + offsetValues;

        if (player.getBlockCountOnPlayer() > playerBlockCount)
        {
            if(cam.fieldOfView < 40)
            {
                cam.fieldOfView += 0.25f;
                playerBlockCount = player.getBlockCountOnPlayer();
            }
        }
        else if (player.getBlockCountOnPlayer() < playerBlockCount)
        {
            if (cam.fieldOfView > 30)
            {
                cam.fieldOfView -= 0.25f;
                playerBlockCount = player.getBlockCountOnPlayer();
            }
        }
        

    }
}
