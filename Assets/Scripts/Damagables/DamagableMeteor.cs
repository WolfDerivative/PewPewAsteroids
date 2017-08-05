using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableMeteor : Damagable {

    protected EMeteor _meteor;

    public override void Start() {
        base.Start();
        _meteor = GetComponent<EMeteor>();
    }//Start

    /// <summary>
    ///   Damage meteor health and velocity. Velocity is reduced by
    /// 0.01% of the incoming dmg.
    /// </summary>
    /// <param name="instigator"> Object that is dealing the damage. </param>
    /// <param name="dmg"> Amount of damage to inflitct. </param>
    /// <returns> True - if meteor was killed, False - still alive. </returns>
    public override bool TakeDamage(GameObject instigator, int dmg) {
        _meteor.SetVelocity(new Vector2(_meteor.Velocity.x - (dmg * 0.01f), _meteor.Velocity.y));
        healthStatus -= dmg;
        if (IsDead) {
            Destroy();
            return true;
        }
        return false;
    }//TakeDamage


    public override void Destroy() {
        base.Destroy();
        _meteor.gameObject.SetActive(false);
    }//Destroy


    public void OnDisable() {
        CancelInvoke();
    }//OnDisable

}//class
