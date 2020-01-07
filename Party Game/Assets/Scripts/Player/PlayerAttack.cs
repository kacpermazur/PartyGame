using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public int playerID = 1;

        public float attackRate = 0.5f;
        public float knockbackForce = 2.0f;
        
        private float _attack;

        public void CollisionDetected(Collider other)
        {
            if (_attack >= 1)
            {
                other.GetComponent<Rigidbody>().AddRelativeForce(transform.right * knockbackForce, ForceMode.Impulse);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            Debug.Log(context.canceled);
            _attack = context.ReadValue<float>();
        }
    }
}