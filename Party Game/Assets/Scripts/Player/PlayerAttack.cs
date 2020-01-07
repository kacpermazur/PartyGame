using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public int playerID = 1;
        
        public float knockbackForce = 10.0f;

        private bool _pressed;
        private bool _released;

        public void CollisionDetected(Collider other)
        {
            if (_pressed==true && _released==false)
            {
                _pressed = false;
                _released = true;
                
                Debug.Log("ATTACK");

                other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.right * knockbackForce, ForceMode.Impulse);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                _pressed = true;
                _released = false;
                GameManager.instance.SoundManager.PlaySound("attack", SoundManager.SoundType.SFX);
            }

            if (context.canceled)
            {
                _pressed = false;
                _released = true;
            }
        }
    }
}