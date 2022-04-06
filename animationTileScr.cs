using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTileScr : MonoBehaviour {

    float targetX;
    float targetY;
    
    Vector3 loc;

    bool active;

    float speed = .8f;
    float error = .4f;

    float xSpeed;
    float ySpeed;

    DeckController ctrObject;

    // Use this for initialization
    void Start () {
        ctrObject = GameObject.Find("Deck").GetComponent<DeckController>();
    }
	
	// Update is called once per frame
	void Update () {
        
            loc = transform.position;
            if (loc.x > targetX - error && loc.x < targetX + error && loc.y > targetY - error && loc.y < targetY + error)
            {

                reportBack();
            }
            else
            {
                //Move
                transform.position = new Vector3(loc.x + xSpeed, loc.y + ySpeed, -.5f);
            }
	}

    //Activate the tile.
    public void initTile(double target_X,double target_Y) {
        
        loc = transform.position;
        targetX = (float) target_X;
        targetY = (float) target_Y;

        xSpeed = targetX - loc.x;
        ySpeed = targetY - loc.y;
        float multi = speed / Mathf.Sqrt(xSpeed * xSpeed + ySpeed * ySpeed);


        xSpeed = xSpeed * multi;
        ySpeed = ySpeed * multi;


    }


    void reportBack() {
        ctrObject.onAnimationFinish();
        Object.Destroy(this.gameObject);
    }
}
