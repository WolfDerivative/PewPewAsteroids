using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipGun : Weapon {

    public bool LookAtMouse = true;

    [Tooltip("Location of the projectile to shoot from. (A spot where projectile is placed.")]
    protected Transform GunBarrel;

    private ProjectilePool _projectilePool;


    public override void Start() {
        base.Start();
        GunBarrel = this.gameObject.transform;
        _projectilePool = GameObject.Find("ProjectilePool").GetComponent<ProjectilePool>();
        if (_projectilePool == null)
            GameUtils.Utils.WarningMessage("Forgot to add ProjectilePool to the scene??");
    }//Start


    public override void Update() {
        base.Update();
        if (LookAtMouse)
            AimAtCursor();
    }


    public override bool Shoot() {
        if (!base.Shoot())
            return false;
        GameObject projectileGO = _projectilePool.GetInactive();
        if (projectileGO == null)
            return false;
        projectileGO.transform.position = GunBarrel.transform.position;
        projectileGO.transform.rotation = GunBarrel.transform.rotation;
        projectileGO.SetActive(true);
        return true;
    }//Shoot


    public void AimAtCursor() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }

}//class
