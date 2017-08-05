using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoundsToCamera : MonoBehaviour {


    [Tooltip("Borders offset from sides of the screen. (0, 0) means 1 to 1 match for the box size and screen.")]
    public Vector2 Offset;

    protected BoxCollider2D _boxCollider;
    private Vector3 prevScreenBounds;

	void Start () {
        _boxCollider = GetComponent<BoxCollider2D>();
        prevScreenBounds = GameUtils.Utils.OrthographicBounds(Camera.main).size;
        this.transform.position = Vector2.zero;
        ResizeBounds(prevScreenBounds);
    }//Start

    void Update () {
        var currSize = GameUtils.Utils.OrthographicBounds(Camera.main).size;
        if (currSize == prevScreenBounds)
            return;
        ResizeBounds(currSize);
    }//Update


    public void ResizeBounds(Vector3 newSize) {
        _boxCollider.size = newSize;
        _boxCollider.size = new Vector2(_boxCollider.size.x + Offset.x, _boxCollider.size.y + Offset.y);
    }//ResizeBounds

}//class
