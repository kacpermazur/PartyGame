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
        public float knockbackForce = 10.0f;
        
        private float _attack;

        private bool _pressed;
        private bool _released;

        public Rigidbody testCase;
        public Rigidbody testCase2;

        public void CollisionDetected(Collider other)
        {
            if (_pressed==true && _released==false)
            {
                _pressed = false;
                _released = true;
                
                Debug.Log("ATTACK");

                other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.right * knockbackForce, ForceMode.Impulse);
                other.GetComponent<TEST>().TestTarget();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                Debug.Log("started: ");
                _pressed = true;
                _released = false;
            }

            if (context.canceled)
            {
                Debug.Log("cancelled");
                _pressed = false;
                _released = true;
            }
            
            //_attack = context.ReadValue<float>();
            //Debug.Log(_attack);
        }
    }
}