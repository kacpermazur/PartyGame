using System;
using Core;
using Sound;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public int playerID = 1;

        public float movementSpeed = 4.0f;
        public float rotationSpeed = 6.0f;
        public float jumpForce = 2.0f;
        public float timeLimit =  2.0f;
        public float dashForce = 5.0f;

        [SerializeField] private float distanceToGround = 2.0f;
        
        private Vector2 _move;
        private float _jump;

        private bool _pressedDash;
        private bool _releasedDash;
        private bool _canDash = true;
        
        private float _timer;
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _timer = timeLimit;
        }

        private void Update()
        {
            if (_pressedDash == true && _releasedDash == false && _canDash)
            {
                _rigidbody.AddForce(transform.right * dashForce, ForceMode.Impulse);
                _canDash = false;
            }

            if (_canDash == false)
            {
                _timer -= Time.deltaTime;

                if (_timer < 0)
                {
                    _canDash = true;
                    _timer = timeLimit;
                }
            }
        }
        
        private void FixedUpdate()
        {
            Movement(_move);
            Jump(_jump);
        }

        public void SetKinamticState(bool state)
        {
            _rigidbody.isKinematic = state;
        }
        
        private void Movement(Vector2 direction)
        {
            Vector3 dir = new Vector3(direction.x,0,direction.y);

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
                
                transform.position += movementSpeed * Time.deltaTime * transform.right;
            }
        }

        public void Jump(float jump)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
                out hit, distanceToGround))
            {
                _rigidbody.AddForce(jump * jumpForce * Vector3.up, ForceMode.Impulse);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _jump = context.ReadValue<float>();
        }
        
        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                _pressedDash = true;
                _releasedDash = false;
                GameManager.instance.SoundManager.PlaySound("attack", SoundManager.SoundType.SFX);
            }

            if (context.canceled)
            {
                _pressedDash = false;
                _releasedDash = true;
            }
        }
    }
}