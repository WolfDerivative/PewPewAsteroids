using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObjects : MonoBehaviour {

    public float Force = 10f;

    private SpaceshipControlls _spaceshipControlls;


    public void Start() {
    }


    public void LateUpdate() {
        if(_spaceshipControlls != null) {
            PullIn(_spaceshipControlls.RB);
        }
    }//Update


    public virtual void PullIn(Rigidbody2D rb) {
        var direction = this.transform.position - rb.gameObject.transform.position;
        rb.AddForce(Force * direction);
    }//PullIn



    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag != GameManager.Instance.GetActiveSpaceship().tag)
            return;

        _spaceshipControlls = GameManager.Instance.GetSpaceshipCmp();
        _spaceshipControlls.SetIsCanMove(false);
    }//OnTriggerExit2D


    public void OnTriggerEnter2D(Collider2D collision) {
        if (GameManager.Instance != null && 
            collision.tag != GameManager.Instance.GetActiveSpaceship().tag)
            return;
        if (_spaceshipControlls == null)
            return;
        _spaceshipControlls.RB.velocity = Vector2.zero;
        PullIn(_spaceshipControlls.RB);
        _spaceshipControlls.SetIsCanMove(true);
        _spaceshipControlls = null;
    }//OnTriggerEnter2D

}//class
