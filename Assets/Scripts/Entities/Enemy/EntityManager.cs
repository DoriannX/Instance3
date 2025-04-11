using UnityEngine;

namespace Entities.Enemy
{
    public class EntityManager : MonoBehaviour
    {
        private int health = 100;
        
        public bool TakeDamage(float damage)
        {
            health -= health;
            
            if (health <= 0)
            {
                gameObject.SetActive(false);
                return true;
            }
            
            return false;
        }
    }
}