using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualJoystick: MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    [Tooltip("Allow stick to move Horizontally. False = no horizontal movement.")]
    public bool EnableHorizontalAxis = true;
    [Tooltip("Allow stick to move Vertically. False = no vertical movement.")]
    public bool EnableVerticalAxis = true;
    [Tooltip("How fast joystick moves.")]
    public Vector2 Sensitivity = new Vector2(2, 2);
    [Tooltip("Horizontal and Vertical value will be set to 0 if it in the DeadZone range.")]
    [Range(0.0f, 0.5f)] public float DeadZone = 0.1f;
    [Tooltip("Actuals borders of the joystick area. Example: circled area doesn't match rect size with the sprite's border.")]
    public Vector2 BorderOffset = new Vector2(3, 3);

    private Image bgArea;
    private Image joystickImg;
    private Vector2 inputVector;


    public void Start() {
        bgArea = GetComponent<Image>();
        joystickImg = this.transform.GetChild(0).GetComponent<Image>();

        inputVector = Vector2.zero;
    }//Start


    public void OnDrag(PointerEventData eventData) {
        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgArea.rectTransform,
                                                                    eventData.position,
                                                                    eventData.pressEventCamera,
                                                                    out pos)) {
            pos.x = pos.x / bgArea.rectTransform.sizeDelta.x;
            pos.y = pos.y / bgArea.rectTransform.sizeDelta.y;

            MoveJoystick(pos);
        }//if
    }//OnDrag


    public void MoveJoystick(Vector2 deltaMovement) {
        this.inputVector.x = (EnableHorizontalAxis) ? deltaMovement.x * Sensitivity.x : 0;
        this.inputVector.y = (EnableVerticalAxis) ? deltaMovement.y * Sensitivity.y : 0;

        this.inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        Vector2 joystickPos = new Vector2(inputVector.x * (bgArea.rectTransform.sizeDelta.x / BorderOffset.x),
                                            inputVector.y * (bgArea.rectTransform.sizeDelta.y / BorderOffset.y));
        joystickImg.rectTransform.anchoredPosition = joystickPos;
    }//AddToJoystick


    public void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }//OnPointerDown


    public void OnPointerUp(PointerEventData eventData) {
    }//OnPointerUp


    public float Horizontal() {
        if (Mathf.Abs(inputVector.x) < DeadZone)
            return 0.0f;
        return inputVector.x;
    }//Horizontal


    public float Vertical() {
        if (Mathf.Abs(inputVector.y) < DeadZone)
            return 0.0f;
        return inputVector.y;
    }//Vertical



    public void OnEnable() {
        Start();
        joystickImg.rectTransform.anchoredPosition = inputVector;
    }//OnDisable


}//class
