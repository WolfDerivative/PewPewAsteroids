using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour {

    public static UserInputManager Instance;

    public InputKey[] Keys = new InputKey[2] {
        new InputKey("Vertical", new KeyCode[] { KeyCode.W }, new KeyCode[] { KeyCode.S }),
        new InputKey("Horizontal", new KeyCode[] { KeyCode.D }, new KeyCode[] { KeyCode.A })
    };

    protected Dictionary<string, InputKey> nameToInput;
    protected Dictionary<KeyCode, string> keyToName;
    protected List<KeyCode> isButtonPressed;


    public void Start() {
        Instance = this;
        nameToInput = new Dictionary<string, InputKey>();
        keyToName = new Dictionary<KeyCode, string>();
        isButtonPressed = new List<KeyCode>();

        foreach (InputKey ik in Keys)
            ik.Init();

        foreach(InputKey key in Keys) {
            if (nameToInput.ContainsKey(key.InputName)) {
                GameUtils.Utils.WarningMessage("Input key '" + key.InputName + "' already been used!");
                continue;
            }//if
            List<KeyCode> keyList = new List<KeyCode>(key.positiveKeys);
            keyList.AddRange(key.negativeKeys);
            foreach(KeyCode k in keyList) {
                if (keyToName.ContainsKey(k))
                    continue;
                keyToName.Add(k, key.InputName);
            }
            nameToInput.Add(key.InputName, key);
        }//foreach
    }//Start


    public void Update() {
        checkKeys();
    }//Update



    /// <summary>
    /// Return True - if inputName key was pressed and not yet released.
    /// False - otherwise.
    /// </summary>
    /// <param name="inputName"> Name of the input regestered in "Keys" array of this class.</param>
    public virtual float IsKeyDown(string inputName) {
        if (!IsInputExist(inputName))
            return 0;

        return nameToInput[inputName].IsPressed;
    }//IsKeyDown


    public virtual bool IsPositiveDown(string inputName) {
        if (!IsInputExist(inputName))
            return false;
        return nameToInput[inputName].IsPositivePressed != 0;
    }//IsPositiveDown


    public virtual bool IsNegativeDown(string inputName) {
        if (!IsInputExist(inputName))
            return false;
        return nameToInput[inputName].IsNegativePressed != 0;
    }//IsPositiveDown


    /// <summary>
    ///  Loop through all inputs in the Keys array and check which
    /// key was pressed or released. Set IsPressed state appropriately.
    /// </summary>
    protected virtual void checkKeys() {

        foreach(KeyCode key in keyToName.Keys) {
            InputKey input = nameToInput[keyToName[key]];
            if (Input.GetKeyDown(key)) {
                input.IsPositivePressed = input.positiveKeys.Contains(key) ? 1 : input.IsPositivePressed;
                input.IsNegativePressed = input.negativeKeys.Contains(key) ? -1 : input.IsNegativePressed;
                continue;
            }
            if (Input.GetKeyUp(key)) {
                input.IsPositivePressed = input.positiveKeys.Contains(key) ? 0 : input.IsPositivePressed;
                input.IsNegativePressed = input.negativeKeys.Contains(key) ? 0 : input.IsNegativePressed;
            }
        }
    }//checkKeys


    /// <summary>
    ///  Return true, if input in the list. False - otherwise and print warning to the console.
    /// This function just saves some laziness of using WarningMessage more than one time on the code.
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public bool IsInputExist(string inputName) {
        if (!nameToInput.ContainsKey(inputName)) {
            GameUtils.Utils.WarningMessage("No such input name '" + inputName + "'!");
            return false;
        }//if
        return true;
    }//IsInputExist

}//class


[System.Serializable]
public class InputKey {

    public string InputName;

    public KeyCode[] Positive;
    public KeyCode[] Negative;

    public List<KeyCode> positiveKeys { get; set; }
    public List<KeyCode> negativeKeys { get; set; }
    public float IsPositivePressed = 0;
    public float IsNegativePressed = 0;
    public float IsPressed { get {
            if (IsPositivePressed != 0)
                return IsPositivePressed;
            return IsNegativePressed;
        } }


    public InputKey(string inputName, KeyCode[] positive, KeyCode[] negative) {
        this.InputName = inputName;
        this.Positive = positive;
        this.Negative = negative;
    }//InputKey


    public void Init() {
        positiveKeys = new List<KeyCode>(Positive);
        negativeKeys = new List<KeyCode>(Negative);
    }

}//InputKey
