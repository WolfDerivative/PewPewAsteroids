using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Gameobject with this game mode is used as a delivery point to
///  where "what" game object must be delivered.
/// </summary>
public class GMDelivery : GameMode {

    [Tooltip("What to deliver to the destionation.")]
    public GameObject What;

    /* -------------------------------------------------- */

    public override void Start() {
        base.Start();
    }//Start


    public void OnTriggerEnter2D(Collider2D collision) {
        var spaceship = GameManager.Instance.GetActiveSpaceship();
        if (collision.name != spaceship.name)
            return;
        var packageToDeliver = spaceship.GetComponentInChildren<Pickupable>();
        if (packageToDeliver == null)
            return;
        if (packageToDeliver.tag.ToLower() != "package")
            return;
        bIsFinifshed = true;
        Debug.Log("Delivered!");
    }//OnTriggerEnter2D

}//class
