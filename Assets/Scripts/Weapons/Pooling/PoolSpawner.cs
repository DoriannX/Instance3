using UnityEngine;
using UnityEngine.Serialization;

namespace Pooling
{
    public class PoolSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("_bulletPrefab")]
        [Header("References")]
        [SerializeField] private GameObject bulletPrefab;

        public Pool<Bullet> pool { get; private set; }

        private void Awake()
        {
            pool = new Pool<Bullet>(() => Instantiate(bulletPrefab).GetComponent<Bullet>(),
                pooledObject => { pooledObject.gameObject.SetActive(true); },
                ResetBullet,
                50,
                10);
        }
                
        private void ResetBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            Transform bulletTransform = bullet.transform;
            bulletTransform.position = Vector3.zero;
            bulletTransform.rotation = Quaternion.identity;
        }
        
        private void OnDestroy()
        {
            // Clean up pool
            pool = null;
        }
    }
}