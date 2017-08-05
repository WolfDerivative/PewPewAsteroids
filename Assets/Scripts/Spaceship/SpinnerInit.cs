using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerInit : MonoBehaviour {

    private bool isSpawned = false;

    void Start () {
        if(GameManager.Instance == null) {
            return;
        }
        GameObject spaceship = GameManager.Instance.SpaceshipPrefab;
        if (spaceship != null)
            spaceship = Instantiate(spaceship);
        var spawnPos = this.transform.position;
        //spaceship.transform.position = new Vector3(spawnPos.x, spawnPos.y, spaceship.transform.position.z);
        spaceship.SetActive(true);
        GameManager.Instance.SetActiveSpaceshipInstance(spaceship);
        isSpawned = true;
	}//Start

    // Update is called once per frame
    void Update() {
        if (!isSpawned) {
            Start();
            Destroy(this.gameObject);
        }
	}//Update

}//class
