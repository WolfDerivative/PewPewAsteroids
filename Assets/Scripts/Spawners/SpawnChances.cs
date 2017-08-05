using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChances : MonoBehaviour {

    // Use INT instead of FLOAT so that random value could be precise between 0 and 100,
    // instead dealing with floating point precisions.
    public int[] Probabilities = new int[10] { 100, 90, 70, 70, 40, 30, 30, 30, 30, 20 };

    /// <summary>
    ///   Return probability for a given index (level).
    /// If index is greater than probabilities length, then return
    /// the last probability from the array.
    /// </summary>
    /// <param name="index"> Index\Level of the Probabilities to retrun the value of. </param>
    /// <returns> Probability for the given index\level. Return last probability 
    /// for index out of range. </returns>
    public int GetProbability(int index) {
        if (index >= Probabilities.Length)
            return Probabilities[Probabilities.Length - 1];
        return Probabilities[index];
    }//GetProbability

}//class
