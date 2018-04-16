using System.Collections;
using System.Collections.Generic;
using UnityEngine;

ppublic abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance = null;

    protected void Awake()
    {
        Instance = this as T;
    }
}

