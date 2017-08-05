using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class JoystickButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {


    public bool IsTouchedDown;


    public void Start() {
        IsTouchedDown = false;
    }//Start


    public void OnPointerDown(PointerEventData eventData) {
        IsTouchedDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        IsTouchedDown = false;
    }


    public void OnDisable() {
        IsTouchedDown = false;
    }//OnDisable

}//class
