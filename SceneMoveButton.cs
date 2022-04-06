using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveButton : MonoBehaviour {

    public string targetScene;
   


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }

    private void OnMouseUp()
    {
        
        SceneManager.LoadScene(targetScene);
    }
}
