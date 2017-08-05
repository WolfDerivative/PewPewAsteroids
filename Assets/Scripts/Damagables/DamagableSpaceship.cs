using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableSpaceship : Damagable {

    protected SpaceshipControlls _spaceship;


    public override void Start() {
        base.Start();
        _spaceship = GetComponent<SpaceshipControlls>();
    }//Start


    public override bool TakeDamage(GameObject instigator, int amount) {
        this.healthStatus -= amount;
        return this.IsDead;
    }

}//class
