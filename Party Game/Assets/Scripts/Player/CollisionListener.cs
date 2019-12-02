using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class CollisionListener : MonoBehaviour
    {
        public int playerID = 1;
        private PlayerAttack _callback;
        private void Start()
        {
            _callback = transform.parent.GetComponent<PlayerAttack>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _callback.CollisionDetected(other);
            }
        }
    }
}