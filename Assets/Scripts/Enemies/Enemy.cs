using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damagable))]
public class Enemy : MonoBehaviour {

    public int Damage = 10;
    [Tooltip("How long before object is destroyed on a certain conditions.")]
    public float LifeSpan = 4f;

    public AudioClip DeathSound;

    protected Damagable _damagable;
    protected AudioSource _audioSource;


    /* -------------------------------------------------------------------- */

    public virtual void Start() {
        _damagable = (_damagable == null) ? GetComponent<Damagable>() : _damagable;
        _audioSource = GetComponent<AudioSource>();
    }


    public virtual void Move(Vector2 direction) {
        transform.Translate(direction, Space.World);
    }//Move


    public virtual void OnEnable() {
        if (_damagable == null)
            Start();
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }//OnEnable


    public virtual void OnDisable() {
        CancelInvoke();
    }//OnDisable


    public virtual void Destroy() {
        this.gameObject.SetActive(false);
    }//OnDestroy


    public virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag != GameManager.Instance.SpaceshipPrefab.tag)
            return;
        var spaceShip = GameManager.Instance.GetSpaceshipCmp();
        spaceShip.DamageDealer.TakeDamage(this.gameObject, Damage);
        _damagable.InstaKill();
        this.gameObject.SetActive(false);
    }//OnTriggerEnter2D

}//class
