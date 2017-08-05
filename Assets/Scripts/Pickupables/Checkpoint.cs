using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : Pickupable {

    protected GMCheckpoints _gameMode;

    /// <summary>
    ///  Register current level's game mode for future reference.
    /// This should be called by GMCheckpoints itself to register itself on Start.
    /// </summary>
    /// <param name="gm">Game mode to register.</param>
    public void RegisterGM(GMCheckpoints gm) { _gameMode = gm; }


    public override void OnTriggerEnter2D(Collider2D collision) {
        base.OnTriggerEnter2D(collision);
        var activeSpaceship = GameManager.Instance.GetActiveSpaceship();
        if (collision.name != activeSpaceship.name)
            return;

        if (_gameMode == null) {
            GameUtils.Utils.WarningMessage("GameMode for '" + this.name + "' was not registered!");
            return;
        }//if

        _gameMode.Pickedup(this);
        Destroy(this.gameObject);
    }//OnTriggerEnter2D

}//class
