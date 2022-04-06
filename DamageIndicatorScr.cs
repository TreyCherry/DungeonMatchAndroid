using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicatorScr : MonoBehaviour
{
     TextMeshPro m_Object;

    float targetX;
    float targetY;

    Vector3 loc;

    bool active;

    float speed = .04f;
    float error = .2f;

    float xSpeed;

    DeckController ctrObject;

    private void Awake()
    {
        m_Object = GetComponent<TextMeshPro>();
        ctrObject = GameObject.Find("Deck").GetComponent<DeckController>();
        m_Object.text = "Empty";
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
            loc = transform.position;
            if (loc.y > targetY - error && loc.y < targetY + error)
            {

                reportBack();
            }
            else
            {
                //Move
                transform.position = new Vector3(loc.x, loc.y + speed, -.5f);
            }
        
    }

    //string is what text says
    public void initText(string Content, Color32 itemColor) {
        //m_Object.color = itemColor;
        m_Object.faceColor = itemColor;


        m_Object.text = Content;
        
        loc = transform.position;

        targetY = loc.y + 2;
    }

    void reportBack() {
        ctrObject.onAnimationFinish();
        Object.Destroy(this.gameObject);
    }
}
