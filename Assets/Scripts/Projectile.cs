using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public int Damage = 10;
    [Tooltip("Time before projectile become inactive again.")]
    public float ProjectileTimeout = 2f;
    [Tooltip("Shooting force to apply to projectile.")]
    public float Force = 500f;

    private Rigidbody2D _rigidBody;
    private List<GameObject> damageRecievers;  //objects that recieved damage during the lifespan of this projectile.
    private LayerMask origLayer;
    private LayerMask inactiveLayer;


    /* ------------------------------------------------------------------------------ */


    public void Start() {
        origLayer = this.gameObject.layer;
        inactiveLayer = LayerMask.NameToLayer("Ignore Raycast");
    }//Start

    public void Update() {
        Bounds cameraBounds = GameUtils.Utils.OrthographicBounds(Camera.main);
        bool[] isInView = GameUtils.Utils.IsWithinBounds(cameraBounds, this.transform.position);
        bool isInCamera = isInView[0] && isInView[1];
        if (!isInCamera)
            Destroy();
    }//Update


    public void FixedUpdate() {

    }//FixedUpdate


    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.ToLower() == "player")
            return;
        if(collision.gameObject.tag.ToLower() == "enemy") {
            var damagable = collision.GetComponent<Damagable>();
            bool isDead = damagable.TakeDamage(this.gameObject, Damage);

            if(isDead)
                GameManager.Instance.ActiveGameMode.AddDestroyedObject(damagable.gameObject);

            this.gameObject.SetActive(false);
        }
    }//OnTriggerEnter2D


    /********* Destroy\Disable handler *********/

    public void OnEnable() {
        if (inactiveLayer == this.gameObject.layer)
            this.gameObject.layer = origLayer;

        if (damageRecievers == null)
            damageRecievers = new List<GameObject>();
        if (_rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();

        var shootForce = this.transform.up * Force;
        _rigidBody.AddForce(shootForce);
        damageRecievers.Clear();
        Invoke("Destroy", ProjectileTimeout);  //despawn after X seconds. Safety net.
    }//OnEnabled


    public void Destroy() {
        this.gameObject.SetActive(false);
    }//Destroy


    public void OnDisable() {
        if (_rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
        CancelInvoke();
    }//OnDisable
}//class
