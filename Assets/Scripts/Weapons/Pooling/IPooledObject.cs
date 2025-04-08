using System;

namespace Pooling
{
    public interface IPooledObject<U> where U : class, IPooledObject<U> 
    {
        void SetReleaseFunc(Action<U> releaseFunc);
    }
}