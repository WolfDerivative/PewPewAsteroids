using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

    public bool IsScreenAsBorder = true;
    public virtual bool IsFinished { get { return bIsFinifshed; } }
    public virtual int DestroyedCount {
        get {
            return allDestroyed.Count;
        }//get
    }//DestroyedCount

    protected bool bIsFinifshed = false;
    protected List<GameObject> allDestroyed;

    /* ------------------------------------------------------------------------------- */

    public virtual void Start() {
        allDestroyed = new List<GameObject>();
    }//Start
    

    public virtual void AddDestroyedObject(GameObject obj) {
        if (allDestroyed == null)
            allDestroyed = new List<GameObject>();
        allDestroyed.Add(obj);
    }//AddDestroiedObject
  

}//class
