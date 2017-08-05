using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour {

    public int Health = 100;
    public bool IsDead { get { return !(healthStatus > 0); } }
    public int HealthStatus { get { return healthStatus; } }

    protected GameObject damageInstigator;
    protected int healthStatus;



    public virtual void Start() {
        healthStatus = Health;
    }//Start


    /// <summary>
    ///   Deal damage to this object.
    /// </summary>
    /// <param name="instigator"> object which is dealing the damage </param>
    /// <param name="amount"> amount of damage dealing</param>
    /// <returns></returns>
    public virtual bool TakeDamage(GameObject instigator, int amount) {
        damageInstigator = instigator;
        return false;
    }//TakeDamage


    /// <summary>
    ///  Reduce health to 0 instantly.
    /// </summary>
    public void InstaKill() {
        healthStatus -= healthStatus;
    }//InstaKill


    public virtual void Destroy() {
        damageInstigator = null;
    }//Destroy

    public virtual void Reset() {
        healthStatus = Health;
    }//Reset

}//class
