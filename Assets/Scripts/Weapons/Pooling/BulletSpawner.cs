using UnityEngine;

namespace Pooling
{
    public class BulletSpawner : MonoBehaviour
    {
        public Pool<Bullet> pool { get; private set; }
        [Header("References")]

        [SerializeField] private GameObject bulletPrefab;
        private Transform transformSpawner;

        private void Start()
        {
            transformSpawner = GetComponent<Transform>();

            pool = new Pool<Bullet>(() => Instantiate(bulletPrefab, transformSpawner).GetComponent<Bullet>(),
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

        public Bullet SpawnBullet()
        {
           return pool.Get();
        }

        private void OnDestroy()
        {
            pool = null;
        }
    }
}