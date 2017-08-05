using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(BoxCollider2D))]
public class UIButtonSelect : MonoBehaviour {


    protected Image _image;
    protected Text _text;
    protected BoxCollider2D _boxCollider;
    protected RectTransform _rectTransform;

    private int origFontSize;
    private int selectedFontSize;


    public void Start() {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();
        _rectTransform = GetComponent<RectTransform>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.size = new Vector2(_rectTransform.rect.size.x, _rectTransform.rect.size.y);

        origFontSize = _text.fontSize;
        selectedFontSize = _text.fontSize + 5;
    }//Start


    public void OnMouseOver() {
        _image.enabled = true;
        _text.fontSize = selectedFontSize;
    }//OnMouseOver


    public void OnMouseExit() {
        _image.enabled = false;
        _text.fontSize = origFontSize;
    }//OnMouseExit


}//class
