namespace Pooling 
{
    public interface IPool<T> 
    {
        int PooledObjectsCount { get; }
        int AliveObjectsCount { get; }

        T Get();
        void Release(T obj);
    }
}