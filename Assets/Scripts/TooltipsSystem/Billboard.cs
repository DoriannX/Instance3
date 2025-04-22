using UnityEngine;

namespace TooltipsSystem
{
    public enum BillboardType
    {
        LookAtCamera, // Full billboard - always face camera
        YAxisOnly, // Only rotate Y-axis towards camera (vertical stays upright)
        LockedAxis // Locks one axis from rotating
    }

    public class Billboard : MonoBehaviour
    {
        [Header("Billboard Settings")] [SerializeField]
        private BillboardType billboardType = BillboardType.LookAtCamera;

        [SerializeField] private bool smoothRotation = true;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Vector3 lockedAxis = Vector3.up; // Used for LockedAxis type
        [SerializeField] private bool useWorldSpace = true; // Toggle between local and world space

        [Header("Scaling Options")] [SerializeField]
        private bool enableDistanceScaling = false;

        [SerializeField] private float minDistance = 5f;
        [SerializeField] private float maxDistance = 30f;
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 1.5f;

        [Header("Offset")] [SerializeField] private Vector3 rotationOffset = Vector3.zero;

        private Transform cameraTransform;
        private Vector3 originalScale;
        private Quaternion targetRotation;

        private void Awake()
        {
            // Cache the camera reference for better performance
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            originalScale = transform.localScale;
        }

        private void Start()
        {
            // Check for main camera if not assigned in Awake
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (cameraTransform == null)
                return;

            // Handle rotation based on billboard type
            switch (billboardType)
            {
                case BillboardType.LookAtCamera:
                    HandleFullBillboard();
                    break;
                case BillboardType.YAxisOnly:
                    HandleYAxisBillboard();
                    break;
                case BillboardType.LockedAxis:
                    HandleLockedAxisBillboard();
                    break;
            }

            // Handle distance-based scaling if enabled
            if (enableDistanceScaling)
            {
                HandleDistanceScaling();
            }
        }

        private void HandleFullBillboard()
        {
            // Get direction from object to camera
            Vector3 directionToObject = transform.position - cameraTransform.position;

            if (useWorldSpace)
            {
                // World space billboard (ignores parent rotation)
                targetRotation = Quaternion.LookRotation(directionToObject, Vector3.up);
            }
            else
            {
                // Local space billboard (accounts for parent rotation)
                Vector3 localDirection = transform.parent != null
                    ? transform.parent.InverseTransformDirection(directionToObject)
                    : directionToObject;

                targetRotation = Quaternion.LookRotation(localDirection, Vector3.up);
            }

            ApplyRotation();
        }

        private void HandleYAxisBillboard()
        {
            Vector3 directionToCamera = transform.position - cameraTransform.position;
            directionToCamera.y = 0; // Zero out Y component

            if (directionToCamera != Vector3.zero)
            {
                if (useWorldSpace)
                {
                    // World space Y-axis billboard
                    targetRotation = Quaternion.LookRotation(directionToCamera);
                }
                else
                {
                    // Local space Y-axis billboard
                    Vector3 localDirection = transform.parent != null
                        ? transform.parent.InverseTransformDirection(directionToCamera)
                        : directionToCamera;

                    targetRotation = Quaternion.LookRotation(localDirection);
                }

                ApplyRotation();
            }
        }

        private void HandleLockedAxisBillboard()
        {
            Vector3 directionToCamera = transform.position - cameraTransform.position;
            Vector3 axisToUse = useWorldSpace
                ? lockedAxis
                : (transform.parent != null ? transform.parent.TransformDirection(lockedAxis) : lockedAxis);

            // Project direction onto plane defined by locked axis
            Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToCamera, axisToUse);

            if (projectedDirection != Vector3.zero)
            {
                if (useWorldSpace)
                {
                    // World space locked-axis billboard
                    targetRotation = Quaternion.LookRotation(projectedDirection, axisToUse);
                }
                else
                {
                    // Local space locked-axis billboard
                    Vector3 localDirection = transform.parent != null
                        ? transform.parent.InverseTransformDirection(projectedDirection)
                        : projectedDirection;

                    Vector3 localAxis = transform.parent != null
                        ? transform.parent.InverseTransformDirection(axisToUse)
                        : axisToUse;

                    targetRotation = Quaternion.LookRotation(localDirection, localAxis);
                }

                ApplyRotation();
            }
        }

        private void ApplyRotation()
        {
            // Apply rotation offset
            targetRotation *= Quaternion.Euler(rotationOffset);

            if (smoothRotation)
            {
                if (useWorldSpace)
                {
                    // Apply world space rotation
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
                else
                {
                    // Apply local space rotation
                    transform.localRotation = Quaternion.Slerp(
                        transform.localRotation,
                        transform.parent != null
                            ? Quaternion.Inverse(transform.parent.rotation) * targetRotation
                            : targetRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
            else
            {
                if (useWorldSpace)
                {
                    transform.rotation = targetRotation;
                }
                else
                {
                    transform.localRotation = transform.parent != null
                        ? Quaternion.Inverse(transform.parent.rotation) * targetRotation
                        : targetRotation;
                }
            }
        }

        private void HandleDistanceScaling()
        {
            float distance = Vector3.Distance(transform.position, cameraTransform.position);
            float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

            // Invert t to make objects larger when closer
            t = 1 - t;

            float scaleMultiplier = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = originalScale * scaleMultiplier;
        }

        // This can be called to force update camera reference if needed
        public void UpdateCameraReference()
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }
    }
}