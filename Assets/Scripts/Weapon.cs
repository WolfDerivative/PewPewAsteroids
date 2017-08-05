using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon: MonoBehaviour {

    public int Damage = 5;
    public float RateOfFire = 2f;
    [Tooltip("Game object that holds the weapon. (typically, a parent of the Weapon object).")]
    public GameObject Owner;

    public LayerMask Attackable;
    public AudioClip AttackSound;

    private AudioSource _audioSource;


    public bool IsCanShoot {
        get {
            return cooldown == RateOfFire;
        }
    }

    protected bool bIsFire;
    protected float cooldown;
    protected bool bIsCooldown;


    // Use this for initialization
    virtual public void Start() {
        bIsFire = false;
        bIsCooldown = false;
        cooldown = 0.0f;
        _audioSource = GetComponent<AudioSource>();
    }//Start


    // Update is called once per frame
    virtual public void Update() {
        if (bIsCooldown) {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0) {
                cooldown = RateOfFire;
                bIsCooldown = false;
            }
        }
    }//Update


    virtual public void FixedUpdate() {
        if (bIsFire) {
            Shoot();
            bIsCooldown = true;
        }//if fire
    }//FixedUpdate

    /* Pulling trigger sets the variable to indicate the "attacking" state
     * which is handled by the FixedUpdate on each frame, while trigger is
     * pulled. */
    virtual public void PullTrigger() {
        bIsFire = true;
    }//OnShoot


    virtual public void ReleaseTrigger() {
        bIsFire = false;
    }//ReleaseTrigger


    /* True - is shot was made. False - otherwise. */
    virtual public bool Shoot() {
        if (!IsCanShoot)
            return false;
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(AttackSound, 1f);
        return IsCanShoot;
    }//Shoot



}//class
