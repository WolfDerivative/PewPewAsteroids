using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundTransformToCamera : MonoBehaviour {

    private Transform prevCameraTransform;


    void Start () {
        prevCameraTransform = Camera.main.transform;
    }//Start
	

	void Update () {
        bool hasTransformChanged = prevCameraTransform.position == Camera.main.transform.position;
        bool hasRotationChanges = prevCameraTransform.rotation == Camera.main.transform.rotation;

        if (hasTransformChanged || hasRotationChanges)
            BoundTransform();
	}//Update


    public void BoundTransform() {
        this.transform.position = Camera.main.transform.position;
        this.transform.rotation = Camera.main.transform.rotation;
    }//BoundTransform

}//class
