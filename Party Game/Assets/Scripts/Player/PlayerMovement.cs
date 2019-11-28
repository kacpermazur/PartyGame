using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public int playerID = 1;

        public float movementSpeed = 4.0f;
        public float rotationSpeed = 6.0f;
        public float jumpForce = 2.0f;

        private Vector2 _move;
        private float _attack;
        private float _jump;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Movement(_move);
            Jump(_jump);
        }

        private void Movement(Vector2 direction)
        {
            Vector3 dir = new Vector3(direction.x,0,direction.y);

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
                
                transform.position += transform.right * movementSpeed * Time.deltaTime;
            }

        }

        public void Jump(float jump)
        {
            _rigidbody.AddForce(Vector3.up * jump * jumpForce, ForceMode.Impulse);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            _attack = context.ReadValue<float>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _jump = context.ReadValue<float>();
        }
    }
}