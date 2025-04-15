using UnityEngine;

namespace Pooling
{
    public class BulletSpawner : MonoBehaviour
    {
        public Pool<Bullet> pool { get; private set; }
        [Header("References")]

        [SerializeField] private GameObject bulletPrefab;
        private Transform bulletSpawnerTransform;

        private void Awake()
        {
            bulletSpawnerTransform = transform;
        }

        private void Start()
        {
            pool = new Pool<Bullet>(() => Instantiate(bulletPrefab, bulletSpawnerTransform.position, Quaternion.identity).GetComponent<Bullet>(),
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

        public Bullet SpawnBullet(int damage, LayerMask hitLayer, Vector3 position, Quaternion rotation)
        {
            Bullet bullet = pool.Get();
            bullet.SetLayer(hitLayer);
            bullet.SetDamage(damage);
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
           return bullet;
        }

        private void OnDestroy()
        {
            pool = null;
        }
    }
}