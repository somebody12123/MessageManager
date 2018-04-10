using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton  {
    private static volatile MessageManager instance = null;
    private static readonly object syncLock = new object();
    public static MessageManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new MessageManager();
                    }
                }
            }
            return instance;
        }
    }
}
