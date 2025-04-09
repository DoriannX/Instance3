using UnityEngine;

namespace Theo.Enemy
{
    public class EntityManager : MonoBehaviour
    {
        public void TakeDamage(float damage)
        {
            Debug.Log($"{name}: TakeDamage({damage})");
        }
    }
}