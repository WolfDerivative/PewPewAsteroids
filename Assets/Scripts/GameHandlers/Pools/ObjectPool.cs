using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool: MonoBehaviour {

    [Tooltip("Game object to be created into the pool.")]
    public GameObject ObjectToPool;
    [Tooltip("Number of objects instanciated on start.")]
    public int PoolSize = 20;
    [Tooltip("Allow pool to grow if there is not enough objects.")]
    public bool IsDynamic = true;
    [Tooltip("How many objects of the pool can be active simultaneously. '0' means no limit.")]
    public int SpawnLimit = 0;

    public virtual int InactiveCount { get {
            int count = 0;
            foreach (GameObject go in this.pool)
                count = (go.activeSelf) ? count : count + 1;
            return count;
        }//get
    }//InactiveCount

    public virtual int ActiveCount {
        get {
            int count = 0;
            foreach (GameObject go in this.pool)
                count = (go.activeSelf) ? count + 1: count;
            return count;
        }//get
    }//ActiveCount


    protected List<GameObject> pool;
    protected GameObject bucket; //parent for all the clones objects.

    /* ------------------------------------------------------------------------------- */

    public virtual void Start() {
        pool = new List<GameObject>();
        createBucket();
        for (int i = 0; i < PoolSize; i++) {
            AddToPool();
        }//for
    }//Start


    public virtual GameObject GetInactive() {
        foreach (GameObject go in this.pool) {
            if (!go.activeInHierarchy)
                return go;
        }//foreach

        //at this point, there is no avaailable objects to pool from.
        //This, create a new one to supply the demand.
        if (!IsDynamic)
            return null;
        return AddToPool();
    }//GetInactive


    /* TODO: docs
     * @return : instantiated game object that has been added to the pool.
     */
    public virtual GameObject AddToPool() {
        GameObject go = (GameObject)Instantiate(ObjectToPool);
        go.name = go.name + "_" + (this.pool.Count + 1);

        if(bucket != null)
            go.transform.SetParent(bucket.transform);

        go.transform.position = new Vector3(0f, 0f, 0f);
        go.transform.localScale = ObjectToPool.transform.localScale;
        go.SetActive(false);
        this.pool.Add(go);
        return go;
    }//AddToPool


    public virtual GameObject SpawnAt(Vector3 spawnAt) {
        if(SpawnLimit > 0) {
            if (ActiveCount >= SpawnLimit)
                return null;
        }
        GameObject go = GetInactive();
        if (go == null)
            return null;
        go.transform.position = spawnAt;
        go.SetActive(true);
        return go;
    }//SpawnAt


    public List<GameObject> GetPool() { return this.pool; }
    

    protected void createBucket() {
        bucket = GameObject.Find("Bucket");
        if (bucket == null) {
            bucket = new GameObject();
            bucket.name = "Bucket";
        }
    }

}//Class