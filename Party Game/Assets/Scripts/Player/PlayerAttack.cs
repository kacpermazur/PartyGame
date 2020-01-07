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

        private bool _pressedAttack;
        private bool _releasedAttack;

        public void CollisionDetected(Collider other)
        {
            if (_pressedAttack==true && _releasedAttack==false)
            {
                _pressedAttack = false;
                _releasedAttack = true;
                
                Debug.Log("ATTACK");

                other.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * knockbackForce, ForceMode.Impulse);
            }
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                _pressedAttack = true;
                _releasedAttack = false;
                GameManager.instance.SoundManager.PlaySound("attack", SoundManager.SoundType.SFX);
            }

            if (context.canceled)
            {
                _pressedAttack = false;
                _releasedAttack = true;
            }
        }
     
    }
}