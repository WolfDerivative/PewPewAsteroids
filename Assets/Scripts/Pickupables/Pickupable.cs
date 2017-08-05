using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Pickupable : MonoBehaviour {

    protected Collider2D _collider;
    protected SpriteRenderer _spriteRenderer;

    /* -------------------------------------------------- */

    public virtual void Start() {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider.isTrigger = true;  //must be trigger.
    }//Start


    public virtual void OnTriggerEnter2D(Collider2D collision) {

    }//OnTriggerEnter2D

}//class
