using UnityEngine;

namespace Theo.Enemy
{
    public class EntityManager : MonoBehaviour
    {
        private int health = 100;
        
        public bool TakeDamage(float damage)
        {
            Debug.Log($"{name}: TakeDamage({damage})");

            health -= health;
            
            if (health <= 0)
            {
                Debug.Log($"{name}: Already dead");
                gameObject.SetActive(false);
                return true;
            }
            
            return false;
        }
    }
}