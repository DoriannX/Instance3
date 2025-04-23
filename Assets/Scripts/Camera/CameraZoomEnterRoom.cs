using UnityEngine;
using UnityEngine.Assertions;

public class CameraZoomEnterRoom : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smooth = 5f;
    [SerializeField] private LayerMask playerLayer;
    private Rooms rooms;
    private bool isZooming = false;

    private void Awake()
    {
        rooms = GetComponentInParent<Rooms>();
        Assert.IsNotNull(mainCamera, "Main Camera component is not assigned.");
    }

    private void Update()
    {
        if(isZooming)
        {
            CameraZoom();

            if (Mathf.Abs(mainCamera.fieldOfView - rooms.zoomCameraData) < 0.01f)
            {
                isZooming = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {            
            if(!isZooming)
            {
                isZooming = true;                     
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isZooming = false;
        }
    }

    private void CameraZoom()
    {
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, rooms.zoomCameraData, smooth * Time.deltaTime);
    }
}
