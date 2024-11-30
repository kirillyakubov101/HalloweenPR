using UnityEngine;

public interface IPoolable <T>
{
    T TryGetPoolObject();
    bool IsPoolFull();
    bool IsPoolEmpty();
}
