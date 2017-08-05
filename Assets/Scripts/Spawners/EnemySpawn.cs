using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;

[RequireComponent(typeof(Wave))]
public class EnemySpawn : MonoBehaviour {
    
    public Wave[] _wave;
    public bool IsRandomTimeout = false;
    [Tooltip("Delay wave spawns in the beginning of the level for that many seconds.")]
    public float LevelStartSpawnTimeout = 2f;

    [Tooltip("Time before the next subwave (and wave) counter is incremented.")]
    public float TimeBeforeNextWave = 0.2f;
    [Tooltip("Time delay between wave/subwave spawns during 'idle' wave routine.")]
    public float SpawnDelay = 0.5f;
    [Tooltip("Optional. Transform towards which should spawned objects move to. Should have box collider attached.")]
    public GameObject MoveTowards;

    public Wave ActiveWave {
        get {
            if (WaveIndex >= _wave.Length) {
                #if UNITY_EDITOR
                    Debug.LogWarning("waveIndex out of range: " + WaveIndex + " | Waves number: " + _wave.Length);
                #endif
                return _wave[_wave.Length - 1];
            }
            return _wave[WaveIndex];
        }
    }//ActiveWave
    public int WaveIndex { get; protected set; }
    public int MaxWaves { get { return _wave.Length -1; } }
    public bool IsLastWave { get { return WaveIndex >= MaxWaves && ActiveWave.IsWaveCleared; } }
    public bool IsStageCleared { get { return ActiveWave.GetActiveObjCount() == 0; } }
    public bool IsNoMoreEnemies { get { return CountActiveEnemies() == 0; } }

    protected ObjectPool[] allPools;
    protected BoxCollider2D _boxCollider;
    protected float currTimeout;
    protected float levelStartTimeout = 1f;
    protected bool isCanSpawn;

    protected float timeSinceWaveStarted = 0f;
    protected float currSpawnDelay = -1f;
    protected int destroyedAtWaveStart = 0;

    private BoxCollider2D _moveTowards;


    /* ******************************************************************************* */


    public virtual void Start() {
        if(_wave.Length == 0)
            _wave = GetComponents<Wave>();
        var poolObj = GameObject.Find("Pools");
        if(poolObj != null)
            allPools = poolObj.GetComponentsInChildren<MeteorPool>();

        _boxCollider = GetComponent<BoxCollider2D>();

        isCanSpawn = false;

        if (MoveTowards)
            _moveTowards = MoveTowards.GetComponent<BoxCollider2D>();
    }//Start


    public virtual void Update() {
        if (MoveTowards) {
        }

        if (levelStartTimeout > 0) { // Do not spawn for the first couple seconds of the level start.
            levelStartTimeout -= Time.deltaTime;
            isCanSpawn = false;
            return;
        }//if

        if (currTimeout > 0) {
            currTimeout -= Time.deltaTime;
            isCanSpawn = false;
            return;
        }//currTimeout

        isCanSpawn = !IsLastWave; //No more spawns, when the last wave is done.

        if (!isCanSpawn)
            return;

        WaveTimers();
        NextWave();  //increse wave when no more subwaves.
    }//Update

    /// <summary>
    ///  Calculate subwave and "idle" timers. "Idle" timer is the one
    /// that controlls the spawn of objects during the Subwave section,
    /// while the subwave timer is the one that will trigger the Next
    /// subwave counter.
    /// </summary>
    protected void WaveTimers() {
        if (timeSinceWaveStarted >= TimeBeforeNextWave) { //add wave progression
            timeSinceWaveStarted = 0.0f;                 //when the time is right
            //ActiveWave.AddProgression(1);
        }

        if (currSpawnDelay == -1 || currSpawnDelay >= SpawnDelay && !IsLastWave) {
            currSpawnDelay = 0.0f;                    // spawn objects when delay
            StartCoroutine(Spawn());                    // exited the time
        }

        if (timeSinceWaveStarted != -1)
            timeSinceWaveStarted += Time.deltaTime;     //increment subwave timer
        if (currSpawnDelay != -1)
            currSpawnDelay += Time.deltaTime;         //increment spawn delay timer
    }//LateUpdate


    public int CountActiveEnemies() {
        int total = 0;
        foreach(MeteorPool pool in allPools) {
            total += pool.ActiveCount;
        Debug.Log(total);
        }
        return total;
    }


    /// <summary>
    ///  Spawn an enemy object inside the bounds of the Collider that is
    /// attached to This gameobject.
    /// </summary>
    /// <param name="toSpawn"> Override Wave's pool pick with user's prefered pool. </param>
    public virtual IEnumerator Spawn(ObjectPool toSpawn = null, int numberOfSpawns = -1) {
        toSpawn = (toSpawn == null) ? ActiveWave.PickRandomPool() : toSpawn;
        numberOfSpawns = (numberOfSpawns == -1) ? ActiveWave.ObjectsPerSpawn : numberOfSpawns;
        for (int i = 0; i < numberOfSpawns; i++) {
            var spawned = toSpawn.SpawnAt(GetRandomPosition(_boxCollider.bounds));
            if(spawned == null) { //happenes when there are no more available objects in to spawn in the pool
                continue;
            }
            EMeteor meteor = spawned.GetComponent<EMeteor>();
            if (meteor) {
                meteor.SetDirection(GetPositionToMoveTowards());
            }

            float waitTime = Random.Range(2, 6) / 10.0f;
            if (numberOfSpawns == 1) //Don't waste cicles on 1 object per spawn.
                waitTime = 0.0f;
            yield return new WaitForSeconds(waitTime);
        }//for
    }//Spawn


    public virtual Vector2 GetPositionToMoveTowards() {
        if (_moveTowards != null)
            return GetRandomPosition(_moveTowards.bounds);
        return Vector2.zero;
    }//GetPositionToMoveTowards


    public virtual Vector2 GetRandomPosition(Bounds randomAt) {
        var left = randomAt.center.x - randomAt.extents.x;
        var right = randomAt.center.x + randomAt.extents.x;

        var top = randomAt.center.y + randomAt.extents.y;
        var bottom = randomAt.center.y - randomAt.extents.y;

        return new Vector2(Random.Range(left, right), Random.Range(top, bottom));
    }//GetRandomPosition


    /// <summary>
    ///  Increase Wave counter when there is no more subwaves.
    /// </summary>
    public void NextWave() {
        if (ActiveWave.IsWaveCleared && !IsLastWave) {
            destroyedAtWaveStart = GameManager.Instance.ActiveGameMode.DestroyedCount;
            WaveIndex++;
        }
    }//NextWave

}//class