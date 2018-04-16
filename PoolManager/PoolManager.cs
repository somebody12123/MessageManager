using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager :MonoBehaviour{
    #region 单例
    public static PoolManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
    }
    #endregion
    //保存对象池
    public Dictionary<string, ObjectPool> pool = new Dictionary<string, ObjectPool>();
    
    //添加一个对象池
    public void AddObjectPool(ObjectPool op)
    {
        string poolName = op.GetType().Name;
        string key = poolName.Replace("ObjectPool", "");
        if(pool.ContainsKey(key))
        {
            Debug.LogWarning("已有该名字的对象池："+poolName);
        }
        else
        {
            pool.Add(key, op);
        }
    }

    //public void AddObjectPool<T>()where T:ObjectPool
    //{
    //    GameObject temp = new GameObject("temp");
    //    ObjectPool op=temp.add
    //}

    //移除一个对象池
    public void RemoveObjectPool(ObjectPool op)
    {
        string poolName = op.GetType().Name;
        string key=poolName.Replace("ObjectPool", "");
        if (pool.ContainsKey(key))
        {
            pool[key].ClearGO();
            pool.Remove(key);
        }else
        {
            Debug.LogWarning("不存在该名字的对象池：" + poolName);
        }
        
    }

    //清除所有对象池
    public void ClearObjectPool()
    {
        foreach(ObjectPool objectPool in pool.Values)
        {
            RemoveObjectPool(objectPool);
        }
    }

    //取出
    public GameObject Spawn(string name,Transform trans)
    {
        GameObject getGO = null;
        if(pool.ContainsKey(name))
        {
            getGO =pool[name].Spawn(trans);
            return getGO;
        }else
        {
            throw new Exception("没有该名字对象的对象池：" + name);
        }
    }

    //放入
    public void Despawn(GameObject go)
    {
        string key = go.name.Replace("(Clone)", "");
        if(pool.ContainsKey(key))
        {
            pool[key].Despawn(go);
        }else
        {
            Debug.LogError("没有该名字对象的对象池：" + key);
        }
    }
}
