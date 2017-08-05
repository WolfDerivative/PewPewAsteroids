using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameState : MonoBehaviour {

    public static UIGameState Instance;

    private UINavigation _uiNav;
    private Image VelocityBar;
    private Rect velocityBarOrigRect;

    public void Start() {
        if (Instance)
            return;
        Instance = this;
        _uiNav = GetComponent<UINavigation>();
        //if (_uiNav.GetElement("VelocityBar")) {
        //    VelocityBar = _uiNav.GetElement("VelocityBar").imgElement;
        //    velocityBarOrigRect = VelocityBar.rectTransform.rect;
        //}
    }//Start


    public void Update() {
        if (_uiNav == null)
            return;
        _uiNav.SetText("Timer", Time.timeSinceLevelLoad.ToString("F2"));

        if (GameManager.Instance.GetSpaceshipCmp() != null) {
            _uiNav.SetText("VelocityValue", GameManager.Instance.GetSpaceshipCmp().Velocity.ToString("F2") + " | " +
                            GameManager.Instance.GetSpaceshipCmp().MaxVelocity.ToString());
        }
    }//Update


    public void LateUpdate() {
        if (GameManager.Instance.GetActiveSpaceship() == null) {
            _uiNav.SetElementActive("GameplayUI", false);
            _uiNav.SetElementActive("MainMenuUI", true);
        } else {
            _uiNav.SetElementActive("GameplayUI", true);
            _uiNav.SetElementActive("MainMenuUI", false);
        }
    }//LateUpdate


    public void UpdateVelocityBar(float val) {
        var barSize = VelocityBar.rectTransform.rect;

        VelocityBar.rectTransform.sizeDelta = new Vector2(val, barSize.height);

        if (VelocityBar.rectTransform.rect.width > velocityBarOrigRect.width)  //TODO: TRASH THIS??
            VelocityBar.rectTransform.sizeDelta = new Vector2(velocityBarOrigRect.x, barSize.height);
    }//UpdateVelocitybar


    public void TogglePauseGame() {
        GameManager.Instance.TogglePauseGame();
    }


    /* Who knows why, but Start is not called on level load! 
* Therefore, assign a delegate on level Enable and Disable, thus
* OnSceneLoaded will be call every time the Scene is Loaded (who knew?).
*/
    public void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }//OnEnable

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveLoad.Instance.SaveGame();
    }//OnDisable

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    }//OnSceneLoaded


}//class
