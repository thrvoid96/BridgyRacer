using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours;

public class FollowPlayerCam : MonoBehaviour
{
    [SerializeField] private CommonBehaviours playerToFollow;
    private Camera cam;
    private int playerBlockCount;
    public Vector3 offsetValues;


    private void Start()
    {
       cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.position = playerToFollow.transform.position + offsetValues;

        if (playerToFollow.blockStackCount > playerBlockCount)
        {
            if(cam.fieldOfView < 40)
            {
                cam.fieldOfView += 0.25f;
                playerBlockCount = playerToFollow.blockStackCount;
            }
        }
        else if (playerToFollow.blockStackCount < playerBlockCount)
        {
            if (cam.fieldOfView > 30)
            {
                cam.fieldOfView -= 0.25f;
                playerBlockCount = playerToFollow.blockStackCount;
            }
        }
        

    }
}
