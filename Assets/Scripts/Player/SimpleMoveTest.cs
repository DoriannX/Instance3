using UnityEngine;

namespace Player
{
    public class SimpleMoveTest : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = transform;
        }

        private void Update()
        {
            Vector3 rotationDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                rotationDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotationDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                rotationDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rotationDirection += Vector3.right;
            }

            if (rotationDirection.normalized.magnitude == 0)
            {
                return;
            }

            playerTransform.forward = Vector3.Lerp(playerTransform.forward, rotationDirection.normalized, Time.deltaTime * rotationSpeed);
            playerTransform.position += playerTransform.forward * (Time.deltaTime * moveSpeed);
        }
    }
}