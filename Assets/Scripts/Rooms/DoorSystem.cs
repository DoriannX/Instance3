using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorSystem : MonoBehaviour
{
    [Header("Reference")]
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }  

    public void OpenDoor()
    {        
        animator.SetTrigger("hasKey");        
    }
}
