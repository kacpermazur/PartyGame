﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerEliminate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
