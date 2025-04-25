using UnityEngine;

namespace Feedback
{
    public class EnemyMoveAnimManager : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private Animator animator;
        private Transform playerTransform;
        private float speed;
        private Vector3 lastPosition;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position;
            speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
            lastPosition = currentPosition;
            
            animator.SetFloat(Speed, speed);
        }
    }
}