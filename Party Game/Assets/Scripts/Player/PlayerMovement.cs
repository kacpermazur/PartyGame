using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public int playerID = 1;

        public float movementSpeed = 4.0f;
        public float rotationSpeed = 6.0f;
        public float jumpForce = 2.0f;

        [SerializeField] private float distanceToGround = 2.0f;
        
        private Vector2 _move;
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

        private void OnEnable()
        {
            _rigidbody.isKinematic = false;
        }

        private void OnDisable()
        {
            _rigidbody.isKinematic = true;
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
    }
}