using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    
}

public abstract class SingletonTemplate<T>:MonoBehaviour
    where T:MonoBehaviour
{
    
    private static T _instance;

    public static T kk
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this as T;
    }
}
