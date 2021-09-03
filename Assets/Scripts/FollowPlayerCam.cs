using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCam : MonoBehaviour
{
    [SerializeField] private GameObject playerToFollow;
    public Vector3 offsetValues;

    void LateUpdate()
    {
        transform.position = playerToFollow.transform.position + offsetValues;
    }
}
