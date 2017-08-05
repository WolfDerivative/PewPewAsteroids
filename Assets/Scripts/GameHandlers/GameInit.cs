using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Instantiate GameManager, Canvas and etc objects from prefabs that
/// are esential fro game control.
/// </summary>
public class GameInit : MonoBehaviour {

    public GameObject[] GameEssentials;

    public void Start() {
        foreach(GameObject go in GameEssentials) {
            if (GameObject.Find(go.name)) {
                GameUtils.Utils.WarningMessage("GameInit found existed instance: " + go.name + ". No action.");
                continue;
            }
            var instance = Instantiate(go);
            instance.name = instance.name.Split('(')[0];
        }
    }//start

}//class
