using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    bool faceup = false;
    int xLoc = 0;
    int yLoc = 0;
    public Sprite back;
    Sprite frontFace;
    
    int cardID = 0;
    string cardID_s = "000";



    string cardName = "";
    string attackText = "";
    string bountyText = "";


    string atrText = "";

    /*
     Atribute shortcuts:

        000:        Null
        001:        Beast
        002:        Slime
        003:        Humanoid
         
         
         */
    string cardAtr_s = "000";

    string attackCode;
    string bountyCode;


    DeckController ctrObject;

    SpriteRenderer sprRenderer;

    bool isVisible = true;


	// Use this for initialization
	void Start () {
        
        //frontFace = Resources.Load("Sprites/TileImages/icon002", typeof(Sprite)) as Sprite;
        sprRenderer = this.GetComponent<SpriteRenderer>();
        ctrObject = GameObject.Find("Deck").GetComponent<DeckController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isVisible) {
            sprRenderer.enabled = false;
        } else if (faceup){
            sprRenderer.enabled = true;
            sprRenderer.sprite = frontFace;
        } else {
            sprRenderer.enabled = true;
            sprRenderer.sprite = back;
        }
        
	}

    public void setLocation(int x, int y) {
        xLoc = x;
        yLoc = y;
    }

    public void flip() {
        faceup = !faceup;
    }

    public void loadTiletoSlot(string newID, string atr, string newAttackCode, string newBountyCode,string newName,
        string newAttackText,string newBountyText, Sprite newSprite) {
        cardID =  int.Parse(newID);
        cardID_s = newID;
        cardAtr_s = atr;
        attackCode = newAttackCode;
        bountyCode = newBountyCode;
        frontFace = newSprite;
        isVisible = true;
        cardName = newName;
        attackText = newAttackText;
        bountyText = newBountyText;
        switch (int.Parse(atr))
        {
            case 0:
                atrText = "";
                break;
            case 1:
                atrText = "Beast";
                break;
            case 2:
                atrText = "Slime";
                break;
            case 3:
                atrText = "Humanoid";
                break;
            case 4:
                atrText = "Trap";
                break;
            case 5:
                atrText = "Weapon";
                break;
            default:
                atrText = "Error";
                break;
        }
    }

    private void OnMouseDown()
    {
        if (!faceup){
                ctrObject.onOtherClick(cardID, xLoc, yLoc);
        }
        
        
    }
    private void OnMouseOver()
    {
        if (faceup) {
            ctrObject.onOtherMouseOver(cardName, attackText, bountyText, atrText);
        }
    }

    //getters
    public string getAttackCode() {
        return attackCode;
    }
    public string getBountyCode() {
        return bountyCode;
    }
    public string getAtr() {
        return cardAtr_s;
    }
    public int getcardID() {
        return cardID;
    }
    public string getCardID_S() {
        return cardID_s;
    }
    public int getXLoc() {
        return xLoc;
    }
    public int getYLoc() {
        return yLoc;
    }
    public Sprite GetFrontFace() {
        return frontFace;
    }
   
    public string getCardName() {
        return cardName;
    }
    public string getAttackText() {
        return attackText;
    }
    public string getBountyText() {
        return bountyText;
    }



    //Hiding and revealing for animations.
    public void hide() {
        isVisible = false;
    }
    public void reveal() {
        isVisible = true;
    }

}
