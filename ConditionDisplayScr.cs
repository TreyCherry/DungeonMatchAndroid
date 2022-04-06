using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDisplayScr : MonoBehaviour
{
    //Health Data
    int visibleCondition = 0;
    Sprite[] collection;
    SpriteRenderer conditionImage;

    DeckController ctrObj;

    void Start()
    {
        collection = new Sprite[4];
        collection[0] = Resources.Load("Sprites/ConditionImages/Healthy", typeof(Sprite)) as Sprite;
        collection[1] = Resources.Load("Sprites/ConditionImages/Invulnerable", typeof(Sprite)) as Sprite;
        collection[2] = Resources.Load("Sprites/ConditionImages/Poisoned", typeof(Sprite)) as Sprite;
        collection[3] = Resources.Load("Sprites/ConditionImages/Regen", typeof(Sprite)) as Sprite;

        conditionImage = this.GetComponent<SpriteRenderer>();
        conditionImage.sprite = collection[0];
    }



    void Update()
    {

    }
        public void updateCondition(int newCondition)
    {
        visibleCondition = newCondition;
        conditionImage.sprite = collection[newCondition];
    }
}
