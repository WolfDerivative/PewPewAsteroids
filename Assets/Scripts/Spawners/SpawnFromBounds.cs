using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUtils;
using System.Linq;

[RequireComponent(typeof(BoxCollider2D))]
public class SpawnFromBounds : EnemySpawn {

    protected int[] lastSpawnBorder; //up, right, down, left
    protected int maxSpawnsPerBorder = 1;


    public override void Start() {
        base.Start();
        resetBorderSpawns();
    }//Start


    public override void Update() {
        base.Update();
        if (!isCanSpawn)
            return;
    }//Update


    public override IEnumerator Spawn(ObjectPool toSpawn = null, int numberOfSpawns = -1) {
        return base.Spawn(toSpawn, numberOfSpawns);
    }//Spawn


    public override Vector2 GetRandomPosition(Bounds randomAt) {
        var picks = this.getAvailabeBorders();
        if (picks.Count == 1)
            resetBorderSpawns();
        int border = Random.Range(0, picks.Count);
        border = picks[border];
        var coords = pickBorder(border);
        var horizontal = Random.Range(Mathf.FloorToInt(coords.xMin), Mathf.FloorToInt(coords.xMax));
        var vertical = Random.Range(Mathf.FloorToInt(coords.yMin), Mathf.FloorToInt(coords.yMax));
        return new Vector2(horizontal, vertical);
    }//GetRandomPosition


    /// <summary>
    ///  Find border that has less than "maxSpawnsPerBorder" number of spawns. If no such available found,
    /// reset border counts and pick one side randomly. Return list of available border (indices).
    /// </summary>
    private List<int> getAvailabeBorders() {
        List<int> available = new List<int>();
        for(int i = 0; i < lastSpawnBorder.Length; i++) {
            if (lastSpawnBorder[i] < maxSpawnsPerBorder)
                available.Add(i);
        }//for
        if (available.Count == 0) {           //PARANOIA! But if no borders where picked, get  random and reset.
            resetBorderSpawns();
            available.AddRange(Enumerable.Range(0, lastSpawnBorder.Length-1));
        }
        return available;
    }//getAvailabeBorders


    private void resetBorderSpawns() {
        lastSpawnBorder = new int[] { 0, 0, 0, 0 };
    }//resetBorderSpawns


    /// <summary>
    ///  Return coords for the border's side. Lock one of the axis to Mix or Max values,
    /// based on the choice of the border.
    /// </summary>
    /// <param name="index"> Border index. 0:top, 1: right, 2:bottom, 3:left. </param>
    private Rect pickBorder(int index) {
        if(index >= lastSpawnBorder.Length)         //Wrong index passed. Recover... dont fail.
            return pickBorder(Random.Range(0, lastSpawnBorder.Length));

        lastSpawnBorder[index]++; //Increment index of the border, to indicate that it has already been used.
        var bounds = GameUtils.Utils.GetCorners(_boxCollider.bounds);

        //Lock vertical and horizontal coords to the Max or Min value,
        //depending on the choice of the border.
        //e.g. Top will lock yMax to yMin, right - locks xMax to xMin.
        if (index == 0)  //top
            bounds.yMax = bounds.yMin;
        if (index == 1)  //right
            bounds.xMax = bounds.xMin;
        if (index == 2)  //bottom
            bounds.yMin = bounds.yMax;
        if (index == 3)  //left
            bounds.xMin = bounds.xMin;

        return bounds;
    }//pickBorder

}//class
