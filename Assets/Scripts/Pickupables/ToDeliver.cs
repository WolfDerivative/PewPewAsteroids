using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDeliver : Pickupable {


    public override void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        var activeSpaceship = GameManager.Instance.GetActiveSpaceship();
        if (collision.name != activeSpaceship.name)
            return;
        this.transform.SetParent(activeSpaceship.transform);
        _spriteRenderer.enabled = false;
    }//OnTriggerEnter2D

}//class
