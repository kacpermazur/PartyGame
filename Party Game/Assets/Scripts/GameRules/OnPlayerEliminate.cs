using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class OnPlayerEliminate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().isAlive = false;
        }
    }
}
