using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    public float Timeout = 1f;

    private Animator _animator;
    private RectTransform _rectTransform;

    public void Start() {
        _rectTransform = GetComponent<RectTransform>();
    }


    public void OnEnable() {
        Invoke("Destroy", Timeout);
        if (_animator == null)
            _animator = GetComponent<Animator>();
        _animator.Play("scorePop");
    }//OnEnable


    public void OnDisable() {
        CancelInvoke();
    }//OnDisable


    public void Destroy() {
        _rectTransform.anchoredPosition = Vector3.zero;
        this.gameObject.SetActive(false);
    }//OnDestroy

}//class
