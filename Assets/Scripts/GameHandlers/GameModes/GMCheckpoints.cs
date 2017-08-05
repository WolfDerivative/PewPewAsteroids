using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMCheckpoints : GameMode {

    public GameObject Checkpoints;
    protected List<Checkpoint> allCheckpoints;


    public override void Start() {
        base.Start();
        allCheckpoints = new List<Checkpoint>(Checkpoints.GetComponentsInChildren<Checkpoint>());
        foreach(Checkpoint cp in allCheckpoints) {
            cp.RegisterGM(this);
        }
    }//Start


    public void Pickedup(Checkpoint cp) {
        if (!allCheckpoints.Contains(cp)) { //Should never happened.
            GameUtils.Utils.WarningMessage("Unregistered checkpoint '" + cp + "' was picked up??");
            return;
        }//if
        allCheckpoints.Remove(cp);
        if (allCheckpoints.Count == 0)
            bIsFinifshed = true;
    }//Pickedup

}//class
