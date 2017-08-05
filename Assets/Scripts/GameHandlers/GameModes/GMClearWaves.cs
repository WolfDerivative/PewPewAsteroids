using UnityEngine;

public class GMClearWaves : GameMode{

    [Tooltip("Name of the GameObject in the scene that spawns enemies.")]
    public string SpawnerName = "EnemySpawner";
    public EnemySpawn Spawner { get; protected set; }
    public override bool IsFinished { get {
            return (Spawner != null) ? Spawner.IsStageCleared && Spawner.IsLastWave && Spawner.IsNoMoreEnemies: false; } }

    /* -------------------------------------------------- */    

    public override void Start() {
        base.Start();
        GameObject spawnerGO = GameObject.Find(SpawnerName);
        if (spawnerGO == null) {
            GameUtils.Utils.WarningGONotFound(SpawnerName);
            return;
        }//if spawnerGO

        Spawner = spawnerGO.GetComponent<EnemySpawn>();
    }//Start


    public override void AddDestroyedObject(GameObject obj) {
        base.AddDestroyedObject(obj);
        Spawner.ActiveWave.AddProgression(1);
    }//AddDestroyedObject


}//class