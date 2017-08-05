using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour {

    public static UINavigation Instance;
    public GameObject[] ShowHideCollection;

    protected Canvas _canvas;
    protected Dictionary<string, UIElementID> uiElements;


    public void Start() {
        if (Instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        FindAllUiElements();
        Instance = this;
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
        if(_canvas.worldCamera == null) {
            _canvas.worldCamera = Camera.main;
        }
        DontDestroyOnLoad(this.gameObject);
    }//Start



    public void LoadScene(int level) {
        SaveLoad.Instance.SaveGame();
        SceneManager.LoadScene(level);
    }//LoadScene


    public void RestartLevel() {
        GameManager.Instance.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame() {
        Application.Quit();
    }


    /// <summary>
    ///  Toggle Show\Hide menu on the screen. This function intended to be used
    /// by UI buttons and suck.
    /// </summary>
    public void ToggleMenu(GameObject toToggle) {
        toToggle.SetActive(!toToggle.activeSelf);
    }//ToggleMenu


    /// <summary>
    ///   Set text of the textField element if it exists.
    /// Do nothing and return False if UIElement does not have a text field.
    /// </summary>
    /// <param name="fieldName">UI element name containing Text component to set text of.</param>
    /// <param name="txt">Text value to set.</param>
    /// <returns>True if field exists and text was set;
    ///          False - no changes and field doesnt exist. 
    /// </returns>
    public bool SetText(string fieldName, string txt) {
        if (!uiElements.ContainsKey(fieldName)) {
#if UNITY_EDITOR
            Debug.LogWarning("Key " + fieldName + " not found!!");
#endif
            return false;
        }
        bool isTextSet = uiElements[fieldName].SetFieldText(txt);
        if(!isTextSet) {
#if UNITY_EDITOR
            //Debug.LogWarning("Text filed for [" + fieldName + "] is not set!!");
            return false;
#endif
        }
        return true;
    }//SetText


    public UIElementID GetElement(string elementName) {
        if (!uiElements.ContainsKey(elementName))
            return null;
        return uiElements[elementName];
    }//GetElement


    public void FindAllUiElements() {
        if(uiElements == null)
            uiElements = new Dictionary<string, UIElementID>();
        foreach (UIElementID element in GetComponentsInChildren<UIElementID>(true)) {
            if(!uiElements.ContainsKey(element.gameObject.name))
                uiElements.Add(element.gameObject.name, element);
        }//foreach
    }//FindAllUiElements


    public void SetElementActive(string menuGoName, bool state) {
        var uiElem = GetElement(menuGoName);
        if (uiElem == null) {
            GameUtils.Utils.WarningMessage(menuGoName + " UIElement not found!");
            return;
        }
        uiElem.gameObject.SetActive(state);
    }//ShowMenu


    public Dictionary<string, UIElementID> GetAllElements() {
        return uiElements;
    }//GetAllElements

    /* Who knows why, but Start is not called on level load! 
     * Therefore, assign a delegate on level Enable and Disable, thus
     * OnSceneLoaded will be call every time the Scene is Loaded (who knew?).
    */
    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }//OnDisable

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
        if(_canvas.worldCamera == null)
            _canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (uiElements == null)
            FindAllUiElements();

        foreach (GameObject go in ShowHideCollection)
            go.SetActive(false);

        if (ShowHideCollection.Length == 0)
            return;
        if (scene.buildIndex == 0)
            ShowHideCollection[0].SetActive(true);
        else
            ShowHideCollection[1].SetActive(true);
    }//OnSceneLoaded


}//class
