using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorPool : ObjectPool {

    private Dictionary<GameObject, EMeteor> _meteorCmp;


    public override void Start() {
        base.Start();
        _meteorCmp = new Dictionary<GameObject, EMeteor>();
        foreach(GameObject go in pool) {
            _meteorCmp.Add(go, go.GetComponent<EMeteor>());
        }
    }//Start

    public override GameObject GetInactive() {
        foreach (GameObject go in this.pool) {
            var cmp = GetMeteorCmp(go);
            if (cmp == null)
                continue;
            if (cmp.IsDisabledThisFrame)
                continue;
            if (!go.activeSelf)
                return go;         
        }//foreach

        //at this point, there is no avaailable objects to pool from.
        //This, create a new one to supply the demand.
        if (!IsDynamic)
            return null;
        return AddToPool();
    }//GetInactive

    public override GameObject AddToPool() {
        GameObject go = base.AddToPool();
        if (_meteorCmp == null)
            _meteorCmp = new Dictionary<GameObject, EMeteor>();
        _meteorCmp.Add(go, go.GetComponent<EMeteor>());
        return go;
    }//AddToPool


    public EMeteor GetMeteorCmp(GameObject go) {
        if (!_meteorCmp.ContainsKey(go)) {
            Debug.LogWarning(go.name);
            return null;
        }
        return _meteorCmp[go];
    }//GetMeteorCmp


    public EMeteor GetMeteorCmp() {
        return _meteorCmp[pool[0]];
    }//GetMeteorCmp

}//class
