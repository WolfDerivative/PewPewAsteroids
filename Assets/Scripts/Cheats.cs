using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {


    private Dictionary<KeyCode, bool> keyPressedStatus;

    public void Start() {
        keyPressedStatus = new Dictionary<KeyCode, bool>();
        keyPressedStatus.Add(KeyCode.LeftControl, false);
        keyPressedStatus.Add(KeyCode.I, false);
        keyPressedStatus.Add(KeyCode.M, false);
    }//Start


    public void Update() {
        if (GameManager.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            KeyStatus(KeyCode.LeftControl, true);

        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            KeyStatus(KeyCode.LeftControl, false);
        }

        if (Input.GetKeyDown(KeyCode.I))
            KeyStatus(KeyCode.I, true);

        if (Input.GetKeyUp(KeyCode.I))
            KeyStatus(KeyCode.I, false);

        if (Input.GetKeyUp(KeyCode.M))
            KeyStatus(KeyCode.M, false);

        if (Input.GetKeyDown(KeyCode.M))
            KeyStatus(KeyCode.M, true);

    }//Update


    public void KeyStatus(KeyCode key, bool status) {
        if (!keyPressedStatus.ContainsKey(key))
            keyPressedStatus.Add(key, status);
        else
            keyPressedStatus[key] = status;
    }//KeyStatus

}//class
