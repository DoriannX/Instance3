using UnityEngine;

namespace Pooling
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _cubePrefab; 
        [SerializeField] private float _spawnInterval = 0.1f;
        [SerializeField] private float _radius = 5.0f;
        
        private Pool<Cube> _pool;
        private float _timer;

        private void Awake()
        {
            _pool = new Pool<Cube>(() => Instantiate(_cubePrefab).GetComponent<Cube>(),
                pooledObject => { pooledObject.gameObject.SetActive(true); },
                ResetCube,
                50,
                10);
        }
        
        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval)
            {
                _pool.Get().transform.position = Random.insideUnitSphere * _radius;
                _timer = 0.0f;
            }
        }
        
        private void ResetCube(Cube cube)
        {
            cube.gameObject.SetActive(false);
            Transform cubeTransform = cube.transform;
            cubeTransform.position = Vector3.zero;
            cubeTransform.rotation = Quaternion.identity;
        }
    }
}