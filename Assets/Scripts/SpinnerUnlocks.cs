using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This script should be attached to the same (or its child) object of
 * the GameManager script.
 */
public class SpinnerUnlocks : MonoBehaviour {

    public static SpinnerUnlocks Instance;

    public GameObject SpinnerBtnList;

    protected Dictionary<string, SpaceshipControlls> spinners;


    public void Start() {
        Instance = this;
        spinners = new Dictionary<string, SpaceshipControlls>();
        foreach(SpaceshipControlls cmp in GetComponentsInChildren<SpaceshipControlls>(true)) {
            this.spinners.Add(cmp.gameObject.name, cmp);
        }//foreach

        RefreshAllSpinnersLocks();
    }//Start


    /* Check if spinner is locked or unlocked and updated its selection button state.
     */ 
    public void RefreshAllSpinnersLocks() {
        foreach(string spinName in this.spinners.Keys) {
            if (!CheckSpinnerStatus(spinName))
                continue;
        }//foreach
    }//


    /* Check if a given spinner is Locked or Unlocked. if unlocked, a selection button
     * "activated" will be set to active and "locked" button will be set to disabled,
     * allowing player to select this spinner (or not).*/
    public bool CheckSpinnerStatus(string spinnerName) {
        //WARNING: yes, hardcoded value here expected of a form "<spinner_name>_btn"
        string spinnerBtnName = spinnerName + "_btn_active";
        UIElementID unlockedSpinnerBtn = UINavigation.Instance.GetElement(spinnerBtnName);

        if (unlockedSpinnerBtn == null) //not a spinner btn element you are looking for
            return false;

        SpaceshipControlls spinnerCmp = this.spinners[spinnerName];
        spinnerBtnName = spinnerName + "_btn_locked";
        UIElementID lockedSpinnerBtn = UINavigation.Instance.GetElement(spinnerBtnName);
        if (lockedSpinnerBtn == null)  // this should never happened, unless development branch.
            return false;

        if (!spinnerCmp.isUnlocked) {
            unlockedSpinnerBtn.gameObject.SetActive(false);
            lockedSpinnerBtn.gameObject.SetActive(true);
            //lockedSpinnerBtn.textField.text = spinnerCmp.name + "\n" + spinnerCmp.pointsToUnlock;
            return true;
        }
        unlockedSpinnerBtn.gameObject.SetActive(true);
        lockedSpinnerBtn.gameObject.SetActive(false);
        return false;
    }//CheckSpinnerStatus

    /* Activate spinner choice button to allow choosing the spinner.
     * This will hide "locked" spinner button and replace it with "active"
     * button.*/
    public void UnlockSpinner(GameObject spinnerGo) {
        if (!spinners.ContainsKey(spinnerGo.name)) {
#if UNITY_EDITOR
            Debug.LogWarning("Cant unlock '" + spinnerGo.name + "'! Not found!");
#endif
            return;
        }//if
        /*
        if (GameManager.Instance.GameState.Currency > spinners[spinnerGo.name].pointsToUnlock) {
            spinners[spinnerGo.name].isUnlocked = true;
            CheckSpinnerStatus(spinnerGo.name);
            GameManager.Instance.GameState.Currency -= spinners[spinnerGo.name].pointsToUnlock;
        }
        */
    }//UnlockSpinner
    

    public Dictionary<string, SpaceshipControlls> GetSpinners() {
        return this.spinners;
    }

}//class
