using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIAnimatorNavigation : MonoBehaviour {

    protected Animator _animator;


    public void Start() {
        _animator = GetComponent<Animator>();
    }//Start


    /// <summary>
    ///  Set animation to a state. Pass "animname_true" to set
    /// bool state of animname to true.
    /// </summary>
    /// <param name="nameAndState"></param>
    public void SetBoolAnim(string nameAndState) {
        bool state = false;
        string[] splited = nameAndState.Split('_');
        if(splited.Length < 2) {
            GameUtils.Utils.WarningMessage("Pass string in the format: animName_true, ti set animName to true.");
            return;
        }
        state = (splited[1].ToLower() == "true") ? true : false;
        _animator.SetBool(splited[0], state);
    }

}//class
