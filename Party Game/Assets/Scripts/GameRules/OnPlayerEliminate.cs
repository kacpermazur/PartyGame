using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Player;
using UnityEngine;

public class OnPlayerEliminate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int playerID = other.GetComponent<PlayerMovement>().playerID;

            GameManager.instance.players[playerID].EliminatePlayer();
        }
    }
}
