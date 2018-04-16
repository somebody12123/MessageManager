using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void Start () {
        PoolManager.Instance.AddObjectPool(GameObject.Find("CubeObjectPool").GetComponent<ObjectPool>());
    }
	
	void Update () {
		if(Input.GetMouseButtonUp(0))
        {
            PoolManager.Instance.Spawn("Cube", transform);
        }else if(Input.GetMouseButtonUp(1))
        {
            PoolManager.Instance.Despawn(GameObject.Find("Cube(Clone)"));
        }
	}
}
