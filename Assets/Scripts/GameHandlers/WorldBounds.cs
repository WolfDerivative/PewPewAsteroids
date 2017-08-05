using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WorldBounds : MonoBehaviour {

    public static WorldBounds Instance;

    public BoxCollider2D Collider {
        get {
            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider2D>();
            return _boxCollider;
        }
    }

    [Tooltip("Case sensitive.")]
    public List<string> TagsToDestroy;

    private BoxCollider2D _boxCollider;

    
    public void Start() {
        Instance = this;
        _boxCollider = GetComponent<BoxCollider2D>();
    }//Start

    public void OnTriggerExit2D(Collider2D collision) {
        if (!TagsToDestroy.Contains(collision.tag))
            return;
        collision.gameObject.SetActive(false);
    }//OnTriggerExit2D

}//class
