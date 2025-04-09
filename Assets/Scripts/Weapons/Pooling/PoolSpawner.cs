using UnityEngine;

namespace Pooling
{
    public class PoolSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _bulletPrefab; 
        
        public Pool<Bullet> _pool;

        private void Awake()
        {
            _pool = new Pool<Bullet>(() => Instantiate(_bulletPrefab).GetComponent<Bullet>(),
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
    }
}