using System;
using DG.Tweening;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f;
    private Transform doorTransform;
    public event Action onDoorOpened;

    protected virtual void Awake()
    {
        doorTransform = transform;
    }

    public void OpenDoor()
    {
        doorTransform.DOMoveY(-doorTransform.lossyScale.y , animationDuration)
            .SetEase(Ease.OutBounce).OnComplete(Disable);
        onDoorOpened?.Invoke();
    }
    
    private void Disable()
    {
        doorTransform.DOKill();
        gameObject.SetActive(false);
        
    }
}
