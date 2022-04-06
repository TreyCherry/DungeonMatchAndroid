using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour {

    int bossMoveX = 0;
    int bossMoveY = 0;
    float timeforTurn;

    public GameObject ctrObj;

    DeckController ctrScr;


    string bossID = "00";
    string bAttCode;
    string bBtyCode;
    SpriteRenderer thisSprite;
    Sprite bossImg;



	// Use this for initialization
	void Start () {
        bossID = InfoBank.bossNum_s;
        thisSprite = this.GetComponent<SpriteRenderer>();
        bossImg = Resources.Load("Sprites/BossImages/boss" + bossID, typeof(Sprite)) as Sprite;
        thisSprite.sprite = bossImg;
        ctrScr = ctrObj.GetComponent<DeckController>();
	}
	
	// Update is called once per frame
	void Update () {


        /*
        if (enemyTurn) {
            timeforTurn -= Time.deltaTime;
            if (timeforTurn <= 0) {
                ctrScr.playerTurn = true;
                enemyTurn = false;
                ctrScr.flipTile(bossMoveX, bossMoveY);
            }
        }
        */

	}


    public void startMove(int X, int Y, string AttCode) {
        bossMoveX = X;
        bossMoveY = Y;
    }

    public int getMoveX() {
        return bossMoveX;
    }
    public int getMoveY()
    {
        return bossMoveY;
    }

    public void setAllBossInfo(string aCode, string bCode) {
        bAttCode = aCode;
        bBtyCode = bCode;
    }

    private void OnMouseOver()
    {
        ctrScr.onBossMouseOver();
    }
}
