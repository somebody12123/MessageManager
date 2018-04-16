using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabPool  {
    public GameObject prefab;
    
    public GameObject LoadPrefab()
    {
        return GameObject.Instantiate(prefab);
    }

    public IEnumerator UnLoadPrefab(GameObject prefab)
    {
        GameObject.Destroy(prefab, 0f);
        yield return 0;
    }
}
