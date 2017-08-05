using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool {

    protected Dictionary<GameObject, Projectile> projectilePool;

    public override void Start() {
        base.Start();
        projectilePool = new Dictionary<GameObject, Projectile>();
        foreach(GameObject go in GetPool()) {
            Projectile projectile = go.GetComponent<Projectile>();
            if(projectile == null) {
                GameUtils.Utils.WarningMessage(go.name + " has no Projectile component!");
                continue;
            }//if
            projectilePool.Add(go, projectile);
        }//foreach
    }//Start


    public Projectile GetProjectileComponent(GameObject obj) {
        if (!projectilePool.ContainsKey(obj)) {
            GameUtils.Utils.WarningMessage(obj.name + " is not in the projectile pool!");
            return null;
        }//if
        return projectilePool[obj];
    }//GetComponent

}//class
