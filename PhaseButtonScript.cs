using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseButtonScript : MonoBehaviour {

    public GameObject ctrObj;
    DeckController ctrScr;

	// Use this for initialization
	void Start () {
        ctrScr = ctrObj.GetComponent<DeckController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        ctrScr.onNextPhaseClick();   
    }
}
