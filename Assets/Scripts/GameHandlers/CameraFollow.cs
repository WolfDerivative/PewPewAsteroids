using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public enum LockAxis { none, x, y, z}

    public float minDistance;
    public float followDistance;
    public Vector3 Offset;
    public float dampTime = 0.15f;
    public Transform target;
    public LockAxis LockedAxis;

    private Camera mainCamera;
    private Vector3 velocity = Vector3.zero;
    private SpaceshipControlls _spaceship;

    /* -------------------------------------------------- */

    public void Start() {
        mainCamera = Camera.main;
    }//Start


    void LateUpdate() {
        if (!target && GameManager.Instance) {
            _spaceship = GameManager.Instance.GetSpaceshipCmp();
            target = _spaceship.gameObject.transform;
            this.transform.position = LockVectorAxis(target.position);
        }
        if (!mainCamera)
            mainCamera = Camera.main;

        if (!target) //noone to follow
            return;

        if (!target)
            return;

        var targetPos = Follow();
        transform.position = LockVectorAxis(targetPos);
    }//FixedUpdate


    public Vector3 Follow() {
        Vector3 moveTowards = target.position;
        return Vector3.SmoothDamp(transform.position, moveTowards + Offset, ref velocity, dampTime);
    }//Follow


    protected Vector3 LockVectorAxis(Vector3 targetPos) {
        Vector3 lockedVector = Vector3.zero;
        if (LockedAxis == LockAxis.x)
            lockedVector = new Vector3(this.transform.position.x, targetPos.y, targetPos.z);
        if (LockedAxis == LockAxis.y)
            lockedVector = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);
        if (LockedAxis == LockAxis.z)
            lockedVector = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);
        return (LockedAxis == LockAxis.none) ? targetPos : lockedVector;
    }

}//class
