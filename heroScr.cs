using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroScr : MonoBehaviour {

    public GameObject ctrObj;
    DeckController ctrScr;
   
    


    // Use this for initialization
    void Start () {
        ctrScr = ctrObj.GetComponent<DeckController>();
        SpriteRenderer sprRenderer = this.GetComponent<SpriteRenderer>();
        string heroNumber = InfoBank.heroNum_s;
        sprRenderer.sprite = Resources.Load("Sprites/HeroImages/hero" +heroNumber, typeof(Sprite)) as Sprite;
    }
	
	// Update is called once per frame
	void Update () {
	    	
	}

    private void OnMouseOver()
    {
        ctrScr.onHeroMouseOver();
    }
}
