using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoad : MonoBehaviour {

    public static SaveLoad Instance;


    public void Start() {
        Instance = this;
    }//Start


    public void SaveGame() {
        //PlayerPrefs.SetInt("Currency", gsd.Currency);
        SaveSpinnerProgress();
    }//SaveGame


    public void SaveSpinnerProgress() {

    }//SaveSpinnerProgress


    public void LoadGame() {
        //GameManager.Instance.GameState.Currency = PlayerPrefs.GetInt("Currency");
    }//LoadGame


    public void ResetProgress(string passcode) {
        if (passcode != "no soup for you")
            return;
#if UNITY_EDITOR
        Debug.Log("Reseting game progress...");
#endif
        PlayerPrefs.DeleteAll();
    }//ResetProgress

}//class