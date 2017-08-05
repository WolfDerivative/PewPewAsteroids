using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISequencing : MonoBehaviour {


    public GameObject[] UIOrder;
    public float Delay = 5f;


    public void Start() {
        this.gameObject.SetActive(false);
    }



    public IEnumerator StartSequence() {
        for(int i = 0; i < UIOrder.Length - 1; i++) {
            UIOrder[i].SetActive(true);
            yield return new WaitForSeconds(Delay);
            UIOrder[i].SetActive(false);
        }
        UIOrder[UIOrder.Length - 1].SetActive(true);
    }


    public void OnEnable() {
        StartCoroutine(StartSequence());
    }//OnEnable


    public void OnDisable() {
        foreach (GameObject go in UIOrder)
            go.SetActive(false);
    }

}//class
