using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{

    int[] levelScores;

    // Start is called before the first frame update
    void Start()
    {


        //Find a way to check if it existed in the past before initilizing. 
        levelScores = new int[5];
    }
}
