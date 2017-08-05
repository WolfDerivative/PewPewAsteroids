using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlopeMath {

    [Tooltip("The increase slope of the upgrade requirement for each next level.")]
    public float Slope = 1;
    [Tooltip("Initial requirement offset state of the upgrade slope. (Typically, Y coord moves up or down)")]
    public float Offset = 0;
    public float MinValue = 0;
    public float MaxValue = 100;


    /// <summary>
    ///  Return Linear value Y based of the input X with controlled by Slope and Offset:
    ///   y = (Slope * x) + Offset
    /// </summary>
    /// <param name="x"> input value for the linear function to calculate Y from. </param>
    /// <returns> Linear result Y based of input X. </returns>
    public float Linear(int x) {
        float y = (Slope * x) + Offset;
        y = (y > MaxValue) ? MaxValue : y;
        y = (y < MinValue) ? MinValue : y;
        return y;
    }//Linear

}//class
