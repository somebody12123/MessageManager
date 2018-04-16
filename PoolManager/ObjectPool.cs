using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理一类的PrefabPool
public abstract class ObjectPool : MonoBehaviour {
    protected int capacity= 0;//隐藏物体列表的容量
    [SerializeField]
    public PrefabPool prefabPool;

    protected List<GameObject> deadPool = new List<GameObject>();//隐藏物体列表
    protected List<GameObject> alivePool = new List<GameObject>();//显示物体列表

    //出对象池
    public virtual GameObject Spawn(Transform trans)
    {
        GameObject getGo = null;
        if (deadPool.Count>0)
        {
            getGo = deadPool[0];
            deadPool.RemoveAt(0);
        }else
        {
            getGo = prefabPool.LoadPrefab();
        }
        getGo.SetActive(true);
        getGo.transform.parent = trans;
        alivePool.Add(getGo);
        
        return getGo;
    }

    //入对象池
    public virtual void Despawn(GameObject go)
    {
        go.SetActive(false);
        go.transform.parent = this.transform;
        alivePool.Remove(go);
        deadPool.Add(go);
        DestroyDeadGO();
    }

    //销毁一些隐藏的物体
    protected void DestroyDeadGO()
    {
        capacity = (alivePool.Count / 2) + 1;
        Debug.Log(capacity);
        while (deadPool.Count > capacity)
        {
            GameObject deadGO = deadPool[0];
            deadPool.RemoveAt(0);
            this.StartCoroutine(prefabPool.UnLoadPrefab(deadGO));
        }
    }

    public void ClearGO()
    {
        foreach(GameObject deadGO in deadPool)
        {
            this.StartCoroutine(prefabPool.UnLoadPrefab(deadGO));
        }
        deadPool.Clear();
        alivePool.Clear();
    }

    #region Unity回调
    private void Start()
    {
        capacity = (alivePool.Count / 2) + 1;
    }
    #endregion
}
