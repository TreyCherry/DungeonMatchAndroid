using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectButton : MonoBehaviour {

    public string selectedChar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseUp()
    {
        InfoBank.heroNum_s = selectedChar;
        SceneManager.LoadScene("InGame");

    }
}
