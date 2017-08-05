using System.Collections.Generic;
using UnityEngine;

/* To complete a wave, progression value must be >= than number of
 * Keys on the curve. Progression can be either time or number of
 *  enemies destroyed.
 */
public class Wave : MonoBehaviour {

    public WaveProperties[] Properties;
    //[Tooltip("Max Objects per subwave spawn dynamics over increase of arbitrary value (e.g. time, kill count...).")]
    //public AnimationCurve SpawnProgression = new AnimationCurve(new Keyframe[] { new Keyframe(1,1), new Keyframe(1, 1) });
    [Tooltip("Number of objects to spawn randomlly between X and Y values.")]
    public Vector2 SpawnRange = new Vector2(1, 2);
    [Tooltip("Value required to be reached to complete the wave.")]
    public float ProgressRequirements = 10;
    public int ObjectsPerSpawn {
        get {
            //return Mathf.FloorToInt(SpawnProgression.Evaluate(progression));
            int min = Mathf.FloorToInt(SpawnRange.x);
            int max = Mathf.FloorToInt(SpawnRange.y);
            return Random.Range(min, max);
        }
    }//ObjectsPerSpawn

    //public int SubwavesCount { get { return SpawnProgression.keys.Length; } }

    /// <summary>
    ///     True if no more subwaves available to spawn. False - more subwaves incoming.
    /// </summary>
    //public bool IsWaveCleared { get { return progression >= SubwavesCount; } }
    public bool IsWaveCleared { get { return progression >= ProgressRequirements; } }

    //Controlls the progress towards the next subwave spawn
    private float progression = 0.0f;


    /* ------------------------------------------------------------------------------ */


    public void Start() {
        if(Properties.Length > 1) // sort properties by SpawnChance in descending (highest first) order. 
            System.Array.Sort(Properties, (x, y) => y.SpawnChance.CompareTo(x.SpawnChance));
    }//Start


    public void Update() {
    }//Update


    /// <summary>
    ///  Pick a pool from the list of pools based of its chances.
    /// </summary>
    /// <returns></returns>
    public ObjectPool PickRandomPool() {
        List<ObjectPool> qualifiedToSpawn = new List<ObjectPool>();
        ObjectPool spawnCandidate = null;
        float highestChance = 0f;

        foreach (WaveProperties property in Properties) {
            float randVal = Random.Range(0, 100);
            if (randVal > property.SpawnChance)
                continue;

            qualifiedToSpawn.Add(property.PoolCmp);
            if (randVal < highestChance)
                continue;

            highestChance = randVal;
            spawnCandidate = property.PoolCmp;
        }//foreach

        if (spawnCandidate == null) {  // None to spawn? Just pick the one with highest chance...
            spawnCandidate = Properties[0].PoolCmp;
        }
        return spawnCandidate;
    }//SpawnByChance


    /// <summary>
    ///  Add to the counter that defines the number of objects that can be spawned at a time.
    /// </summary>
    /// <param name="amount"></param>
    public virtual void AddProgression(float amount) {
        progression += amount;
    }

    public virtual void ResetProgression() { progression = 0.0f; }
    public virtual float GetProgression() { return this.progression; }

    public virtual int GetActiveObjCount() {
        int total = 0;
        foreach (WaveProperties wp in Properties) {
            total += wp.PoolCmp.ActiveCount;
        }
        return total;
    }//GetActiveObjCount

}//Wave


[System.Serializable]
public class WaveProperties {

    [Tooltip("Object pools to pick from for this wave based of its chances.")]
    public GameObject PoolObj;
    [Range(0.0f, 1.0f)]
    public float SpawnChance = 0.5f;

    public ObjectPool PoolCmp {
        get {
            if (_pool == null)
                init();
            return _pool;
        }
    }

    private ObjectPool _pool;


    private void init() {
        if (PoolObj == null) {
            #if UNITY_EDITOR
                Debug.LogWarning("Pool object is not set!");
            #endif
            return;
        }
        _pool = PoolObj.GetComponent<ObjectPool>();
    }//Start
 
}//class
