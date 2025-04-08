using UnityEngine;

namespace PlayerCamera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float followSmoothSpeed = 5f;
        private Vector3 targetPosition;
        private Transform cameraTransform;
        private float moveTimer = 0f;
        [SerializeField] private float timeToFullOffset = 1.0f;

        private void Awake()
        {
            cameraTransform = transform;
            targetPosition = target.position;
        }
        
        private void Update()
        {
            bool isTargetMoving = Mathf.Abs((targetPosition - target.position).magnitude) > 0.01f;
            targetPosition = target.position;

            if (isTargetMoving)
            {
                moveTimer = Mathf.Min(moveTimer + Time.deltaTime, timeToFullOffset);
            }
            else
            {
                moveTimer = 0f;
            }

            float offsetRatio = moveTimer / timeToFullOffset;
            Vector3 offsetPosition = target.position + target.TransformDirection(offset);
            Vector3 desiredPosition = Vector3.Lerp(target.position, offsetPosition, offsetRatio);
    
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, Time.deltaTime * followSmoothSpeed);
        }
    }
}
