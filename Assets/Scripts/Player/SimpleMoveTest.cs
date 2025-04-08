using UnityEngine;

namespace PlayerTest
{
    public class SimpleMoveTest : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = transform;
        }

        private void Update()
        {
            Vector3 _moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                _moveDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _moveDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _moveDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _moveDirection += Vector3.right;
            }
            playerTransform.position += _moveDirection.normalized * (Time.deltaTime * moveSpeed);
        }
    }
}