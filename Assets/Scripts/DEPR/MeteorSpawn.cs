using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEPR
public class MeteorSpawn : EnemySpawn {

    private int maxContinuousSpawn = 3;
    private int currContinuousSpawnCount = 0;
    private float maxContiniousSpawnCooldown = 1f;
    private float currContinuousSpawnTime;

/* ------------------------------------------------------------------------------- */

    public override void Start() {
        base.Start();
        currContinuousSpawnTime = maxContiniousSpawnCooldown;
    }//Start


    public override void Update() {
        base.Update();
        _boxCollider.size = GameUtils.Utils.OrthographicBounds(Camera.main).size;
        _boxCollider.size = new Vector2(_boxCollider.size.x + 10, _boxCollider.size.y + 10);

        currContinuousSpawnTime -= Time.deltaTime;
        if (!isCanSpawn) {
            currContinuousSpawnCount = 0;
            currContinuousSpawnTime = maxContiniousSpawnCooldown;
            return;
        }

        if(currContinuousSpawnCount >= maxContinuousSpawn) {
            if(currContinuousSpawnTime <= 0f) {  // Reset Spawn Restrictions; Allow spawn from this point again.
                currContinuousSpawnCount = 0;
                currContinuousSpawnTime = maxContiniousSpawnCooldown;
                return;
            }
            Debug.Log(this.name + " Cant Spawn!");
            return;
        }//if

        SpawnByChance();
        currContinuousSpawnCount++;
        currContinuousSpawnTime = maxContiniousSpawnCooldown;
    }//Update


    public ObjectPool SpawnByChance() {
        List<MeteorPool> qualifiedToSpawn = new List<MeteorPool>();
        ObjectPool spawnCandidate = null;
        float highestChance = 0f;
        foreach (MeteorPool pool in allPools) {
            EMeteor meteorCmp = pool.GetMeteorCmp();
            if(meteorCmp == null || meteorCmp.Chances == null) {
#if UNITY_EDITOR
                Debug.LogWarning("MeteorSpawn failed to get meteor component or meteor chances!!" +
                                "\n Cmp: " + meteorCmp + "; Chances: " + meteorCmp.Chances);
#endif
                meteorCmp.Start();
            }

            float randVal = Random.Range(0, 100);

            qualifiedToSpawn.Add(pool);
            if (randVal < highestChance)
                continue;

            highestChance = randVal;
            spawnCandidate = pool;
        }//foreach

        if (spawnCandidate == null) {
            if (qualifiedToSpawn.Count > 0) {
                spawnCandidate = qualifiedToSpawn[Random.Range(0, qualifiedToSpawn.Count - 1)];
            }else {                                //Paranoia! This should never happened!
                //spawnCandidate = _meteorPools[0];  // Pick first pool to be "safe" to spawn at least something.
#if UNITY_EDITOR
                Debug.LogWarning("MeteorSpawn couldnt pick pool to spawn from!!");
#endif
            }
        }

        //StartCoroutine(Spawn(spawnCandidate));
        return spawnCandidate;
    }//SpawnByChance


}//class
