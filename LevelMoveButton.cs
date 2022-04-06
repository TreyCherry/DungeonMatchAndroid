using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelMoveButton : MonoBehaviour {
    public string selectedBoss;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseUp()
    {
        InfoBank.bossNum_s = selectedBoss;
        SceneManager.LoadScene("CharSelectScene");

    }
}
