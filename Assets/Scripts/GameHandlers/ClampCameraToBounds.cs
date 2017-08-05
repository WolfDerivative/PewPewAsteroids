using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampCameraToBounds : MonoBehaviour {

    public GameObject WorldBounds;
    [Tooltip("WorldBounds - Offset.")]
    public Vector2 Offset;

    private BoxCollider2D _worldBounds;
    private Camera _camera;


    public void Start() {
        _worldBounds = WorldBounds.GetComponent<BoxCollider2D>();
        _camera = GetComponent<Camera>();
        if (_worldBounds == null)
            GameUtils.Utils.WarningMessage("WorldBounds GO has no BoxCollider2D!");
    }//Start


    public void LateUpdate() {
        ClamCamera();
    }//Update


    public void ClamCamera() {
        var clampedPos = Vector2.zero;
        var min = _worldBounds.bounds.min;
        var max = _worldBounds.bounds.max;

        var height = _camera.orthographicSize;
        var width = _camera.aspect * height;

        var clamedPos = transform.position;
        clamedPos.x = Mathf.Clamp(clamedPos.x, min.x + width + Offset.x, max.x - width - Offset.x);
        clamedPos.y = Mathf.Clamp(clamedPos.y, min.y + height + Offset.y, max.y - height - Offset.y);
        this.transform.position = clamedPos;
    }//ClamCamera

}//class
