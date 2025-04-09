using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Pooling 
{
    public class Pool<T> : IPool<T> where T : class, IPooledObject<T> 
    {
        private readonly Func<T> createFunc;
        private readonly Action<T> onGetFunc;
        private readonly Action<T> onReleaseFunc;
        private readonly Stack<T> pooledObjects;

        private int aliveObjectsCount;

        public int PooledObjectsCount => pooledObjects.Count;

        public int AliveObjectsCount => aliveObjectsCount;

        public Pool(Func<T> createFunc, int capacity = 50, int preAllocationCount = 0) : this(createFunc, null, null, capacity, preAllocationCount) {}

        public Pool(Func<T> createFunc, Action<T> onGetFunc, Action<T> onReleaseFunc, int capacity = 50, int preAllocationCount = 0) 
        {
            Assert.IsNotNull(createFunc, "The object creation function can't be null.");
            Assert.IsTrue(capacity >= 1, "The capacity of the pool must be greater than or equal to 1.");
            Assert.IsTrue(preAllocationCount >= 0, "The pre-allocation count of the pool must be greater than or equal to 0.");

            pooledObjects = new Stack<T>(capacity);
            this.createFunc = createFunc;
            this.onGetFunc = onGetFunc;
            this.onReleaseFunc = onReleaseFunc;
            aliveObjectsCount = 0;
            PreAllocatePooledObjects(preAllocationCount);
        }

        private void PreAllocatePooledObjects(int preAllocationCount) 
        {
            for (int i = 0; i < preAllocationCount; i++)
            {
                T pooledObject = CreatePoolObject();
                Release(pooledObject);
            }
        }

        public T Get() 
        {
            T pooledObject = pooledObjects.Count > 0 ? pooledObjects.Pop() : CreatePoolObject();

            aliveObjectsCount++;

            onGetFunc?.Invoke(pooledObject);
            return pooledObject;
        }

        private T CreatePoolObject() 
        {
            T pooledObject = createFunc.Invoke();
            Assert.IsNotNull(pooledObject, "The object to create can't be null.");
            pooledObject.SetReleaseFunc(Release);
            return pooledObject;
        }

        public void Release(T pooledObject) 
        {
            Assert.IsNotNull(pooledObject, "The object to release can't be null.");
            pooledObjects.Push(pooledObject);
            aliveObjectsCount--;
            onReleaseFunc?.Invoke(pooledObject);
        }
    }
}