using UnityEngine;

public class LightAlert : MonoBehaviour
{
    private Light lightComponent;
    [SerializeField] private Color lightDefaultColor = Color.white;
    [SerializeField] private Color lightAlarmColor = Color.red;
    [SerializeField] private float speed = 1.0f;

    private void Awake()
    {
        lightComponent = GetComponent<Light>();
    }

    private void Update()
    {
        lightComponent.color = Color.Lerp(lightDefaultColor, lightAlarmColor, Mathf.PingPong(Time.time, 1 / speed));
    }
}