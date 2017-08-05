using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager   Instance;

    public GameData GameState;
    [Tooltip("Spaceship prefab object that will be controlled by player.")]
    public GameObject SpaceshipPrefab;

    public bool IsGameOver      { get { return isGameOver; } }

    public BoxCollider2D WorldBounds {
        get {
            if (_worldBounds == null) {
                _worldBounds = Camera.main.gameObject.GetComponentInChildren<BoxCollider2D>();
            }
            return _worldBounds;
        }
    }//WorldBounds

    public GameMode ActiveGameMode {
        get {
            if (_activeGameMode == null) {
                GameObject gamemodeGO = GameObject.Find("GameMode");
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return null;
                if (gamemodeGO == null)
                    GameUtils.Utils.WarningGONotFound("GameMode");
                else
                    _activeGameMode = gamemodeGO.GetComponent<GameMode>();
            }//if null
            return _activeGameMode;
        }
    }//ActiveGameMode


    private SpaceshipControlls _activeSpaceship;
    private float              origTimeScale;
    private bool               isGameOver;
    private bool               isSlowmo;
    private float              slowmoTime = 1f; //time for slowmotion on gameover
    private GameMode           _activeGameMode;
    private GameObject         _activeSpaceshipInstance;
    private BoxCollider2D      _worldBounds;


    // Use this for initialization
    public void Start () {
        isGameOver = false;
        isSlowmo = false;
        if (Instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        Instance = this;

        //Load values into GameState inside the LoadGame function. It will use GM's Instance.
        SaveLoad.Instance.LoadGame();
        if (SpaceshipPrefab != null)
            _activeSpaceship = GetComponent<SpaceshipControlls>();
        origTimeScale = Time.timeScale;
        

        DontDestroyOnLoad(this.gameObject);
	}//start


    public void Update() {
        if (isSlowmo) {
            onGameOver();
        }//isSlowmo

        if (isGameOver) {
            GameObject gameOverMenu = UINavigation.Instance.GetElement("GameOverMenu").gameObject;
            gameOverMenu.SetActive(true);
            return;
        }//isGameOver

        //if (Input.GetButtonDown("ResetProgress")) {
        //    //GameState.ResetProgress();
        //    SaveLoad.Instance.ResetProgress("no soup for you");
        //}//if reset

        if(ActiveGameMode != null) {
            if (ActiveGameMode.IsFinished)
                UINavigation.Instance.SetElementActive("StageComplete", true);
            else
                UINavigation.Instance.SetElementActive("StageComplete", false);
        }
    }//Update


    public void SetSpinner(GameObject spin) {
        SpaceshipPrefab = spin;
        _activeSpaceship = SpaceshipPrefab.GetComponent<SpaceshipControlls>();
    }//SetSpinner


    public GameObject GetActiveSpaceship() { return _activeSpaceshipInstance; }//GetActiveSpaceship

    /// <summary>
    ///  Set spaceship instance. Should be called by SpaceshipInit script, or any other that instanciates
    /// spaceship object.
    /// </summary>
    /// <param name="spaceship"></param>
    public void SetActiveSpaceshipInstance(GameObject spaceship) { _activeSpaceshipInstance = spaceship; }


    public SpaceshipControlls GetSpaceshipCmp() {
        if (_activeSpaceship == null) {
            if (GetActiveSpaceship() != null)
                _activeSpaceship = GetActiveSpaceship().GetComponent<SpaceshipControlls>();
        }
        return _activeSpaceship;
    }


    public void TogglePauseGame() {
        if(Time.timeScale > 0) {
            origTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
        }else if (Time.timeScale == 0) {
            Time.timeScale = origTimeScale;
        }
    }//PauseGame


    public void GameOver() {
        isSlowmo = true;
    }//GameOver


    protected void onGameOver() {
        Time.timeScale = 0.5f;
        slowmoTime -= Time.deltaTime;
        if(slowmoTime <= 0) {
            isGameOver = true;
            //Time.timeScale = 1f;
        }
    }//onGameOver


    public void Reset() {
        isGameOver = false;
        isSlowmo = false;
        slowmoTime = 1;
        Time.timeScale = 1f;

        if(GetSpaceshipCmp() != null)
            GetSpaceshipCmp().Reset();
        
        if(UINavigation.Instance)
            UINavigation.Instance.SetElementActive("GameOverMenu", false);

        _activeGameMode = null;
        _activeSpaceship = null;
    }//Reset


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
        Reset();
        if (scene.buildIndex == 0)
            SpaceshipPrefab.SetActive(false);

        if (scene.buildIndex == 0)  // Levels (non main menu) related routines
            return;
    }//OnSceneLoaded

}//class


[System.Serializable]
public class GameData {
    /*
    public int Currency; //Every N's click makes up a Sequence
    public int CreatureLevel;
    public int FarmLevel;

    public void ResetProgress() {
        CreatureLevel = 0;
        FarmLevel = 0;
        Currency = 0;
    }//ResetProgress
    */
}//GameStateData