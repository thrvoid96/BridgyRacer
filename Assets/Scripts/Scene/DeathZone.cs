using Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] FinishGame finishGame;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<CommonBehaviours>();

            if (player.getPlayerNum == 0)
            {
                finishGame.EndGame();
            }
        }
    }
}
