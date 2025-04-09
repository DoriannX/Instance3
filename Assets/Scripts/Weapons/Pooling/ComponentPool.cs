using UnityEngine;
using UnityEngine.Assertions;

namespace Pooling 
{
    public class ComponentPool<T> : IPool<T> where T : Component, IPooledObject<T> 
    {
        private readonly Pool<T> _pool;

        public int PooledObjectsCount => _pool.PooledObjectsCount;

        public int AliveObjectsCount => _pool.AliveObjectsCount;

        public ComponentPool(GameObject prefab, int capacity = 50, int preAllocationCount = 0) 
        {
            Assert.IsNotNull(prefab, "The prefab can't be null.");
            _pool = new Pool<T>(
                () => {
                    GameObject gameObject = Object.Instantiate(prefab);
                    return gameObject.GetComponent<T>();
                },
                (pooledObject) => { pooledObject.gameObject.SetActive(true); },
                (pooledObject) => { pooledObject.gameObject.SetActive(false); },
                capacity,
                preAllocationCount);
        }

        public T Get() 
        {
            return _pool.Get();
        }

        public void Release(T obj) 
        {
            _pool.Release(obj);
        }
    }
}