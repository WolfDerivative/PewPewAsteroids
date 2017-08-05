using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class uses SoundClip and Particle systme instantiations to allow
/// playback of sounds and particles outside of the destroyed objects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ExplosionEffect : MonoBehaviour {

    public ParticleSystem ExplosionParticle;
    public AudioClip Sound;


    protected AudioSource _audioSource;

    private float           timeBeforeDestroy;
    private ParticleSystem  particleInstance; //Particle must be Instantiated from Prefab.
    private bool            isLifeSpan; //used to detect Particle instnce lifespan


    /// <summary>
    ///  Called after OnEnable. Thus, don't set timeBeforeDestroy and isLifeSpan
    /// here!
    /// </summary>
	void Start () {
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
	}//Starts


    public void LateUpdate() {
        if (!isLifeSpan)  //isLifeSpan set by OnEnable
            return;
        timeBeforeDestroy -= Time.deltaTime;
        if(timeBeforeDestroy < 0) {
            Destroy(particleInstance.gameObject);
            Destroy(this.gameObject);
        }
    }//Update


    public virtual void OnEnable() {
        Start();
        PlaySound();
        PlayEffect();

        timeBeforeDestroy = (Sound.length > ExplosionParticle.main.duration) ? Sound.length : ExplosionParticle.main.duration;
        isLifeSpan = true;  //this will start countdown before this GO will be destroyed.
    }//OnEnable


    public virtual void PlaySound() {
        if (Sound == null) {
            GameUtils.Utils.WarningMessage("Sound for '" + this.name + "' was not set!");
            return;
        }
        _audioSource.PlayOneShot(Sound);
    }//PlaySound


    public virtual void PlayEffect() {
        if (ExplosionParticle == null) {
            GameUtils.Utils.WarningMessage("ParticleSystem for '" + this.name + "' was not set!");
            return;
        }
        particleInstance = Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation);
    }//PlayEffect

}//class
