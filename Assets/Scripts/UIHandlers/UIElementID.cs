using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementID : MonoBehaviour {

    public Text textField;
    public Button btnElement;
    public Image imgElement;


    public void Start() {
        if (textField == null)
            textField = GetComponent<Text>();

        if (btnElement == null)
            btnElement = GetComponent<Button>();

        if (imgElement == null)
            imgElement = GetComponent<Image>();


        if(btnElement != null) {
            textField = GetComponentInChildren<Text>(true);
        }
    }//Start

    public void OnEnable() {
        Start();
    }//OnEnable


    /// <summary>
    ///   Set text of the textField element if it exists.
    /// Do nothing and return False if UIElement does not have a text field.
    /// </summary>
    /// <param name="txt">Text value to set.</param>
    /// <returns>True if field exists and text was set;
    ///          False - no changes and field doesnt exist. 
    /// </returns>
    public bool SetFieldText(string txt) {
        if (textField == null)
            return false;
        textField.text = txt;
        return true;
    }//SetFieldText

}//class
