using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RedZone : MonoBehaviour {

    public int Damage = 20;
    [Tooltip("Interval between damage.")]
    public float Interval = 1.0f;

    private List<Damagable> damagablesEntered;
    private float countdown;

    public void Start() {
        damagablesEntered = new List<Damagable>();
        countdown = Interval;
    }//Start


    public void OnTriggerStay2D(Collider2D collision) {
        if (damagablesEntered.Count == 0)
            return;
        countdown += Time.deltaTime;
        if (countdown < Interval)
            return;
        countdown = 0;
        foreach(Damagable dmgbl in damagablesEntered) {
            Debug.Log("Damaging " + dmgbl.name + ". Status: " + dmgbl.HealthStatus);
            dmgbl.TakeDamage(this.gameObject, Damage);
        }//foreach
    }//OnTriggerStay2D


    public void OnTriggerEnter2D(Collider2D collision) {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable == null)
            return;
        damagablesEntered.Add(damagable);
    }//OnTriggerEnter2D


    public void OnTriggerExit2D(Collider2D collision) {
        var damagable = collision.GetComponent<Damagable>();
        if (damagable == null)
            return;
        if (!damagablesEntered.Contains(damagable))
            return;
    }

}//class
