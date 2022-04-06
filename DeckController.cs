using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using TMPro;

public class DeckController : MonoBehaviour {

    struct cardInfo {
        string cardID_s;
        int cardID_i;

        //metadata
        string cardName;
        string attackText;
        string bountyText;
        string atrText;

        string attackCode;
        string bountyCode;
        string attribute;
        Sprite face;

        public cardInfo(string cID) {
            cardID_s = cID;
            cardID_i = int.Parse(cID);
            attackCode = "";
            bountyCode = "";

            cardName = "";
            attackText = "";
            bountyText = "";
            atrText = "";

            attribute = "000";
            face = Resources.Load("Sprites/TileImages/icon" + cID, typeof(Sprite)) as Sprite;
        }

        public cardInfo(string cID, string attrib, string attCode, string btyCode)
        {
            cardID_s = cID;
            cardID_i = int.Parse(cID);
            attackCode = attCode;
            bountyCode = btyCode;

            cardName = "";
            attackText = "";
            bountyText = "";
            atrText = "";

            attribute = attrib;
            face = Resources.Load("Sprites/TileImages/icon" + cID, typeof(Sprite)) as Sprite;
        }

        public cardInfo(string cID, string iName, string iAttack, string iBounty, string attrib, string attCode, string btyCode)
        {
            cardName = iName;
            attackText = iAttack;
            bountyText = iBounty;
            switch (int.Parse(attrib)) {
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

            cardID_s = cID;
            cardID_i = int.Parse(cID);
            attackCode = attCode;
            bountyCode = btyCode;

            attribute = attrib;
            face = Resources.Load("Sprites/TileImages/icon" + cID, typeof(Sprite)) as Sprite;
        }

        public cardInfo(TileScript tScr) {
            cardID_s = tScr.getCardID_S();
            cardID_i = tScr.getcardID();
            attackCode = tScr.getAttackCode();
            bountyCode = tScr.getBountyCode();
            attribute = tScr.getAtr();
            face = tScr.GetFrontFace();

            cardName = tScr.getCardName();
            attackText = tScr.getAttackText();
            bountyText = tScr.getBountyText();
            switch (int.Parse(tScr.getAtr()))
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



        //Getters
        public string getCardID() {
            return cardID_s;
        }
        public string getAttackCode() {
            return attackCode;
        }
        public string getBountyCode()
        {
            return bountyCode;
        }
        public Sprite getFace() {
            return face;
        }
        public string getCardAtr() {
            return attribute;
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
    };

    struct CardDisplay {

        //Info to display
        
        //Links to textmeshPros
        TextMeshPro display;

        //String should be "CardDisplay"
        public CardDisplay(string dispName) {
            display = GameObject.Find(dispName).GetComponent<TextMeshPro>();
            display.text = "";
        }

        //For bosses
        public void updateDisplay(string iCardName, string iCardText, int iHealth) {
            display.text = iCardName + "\n--------------------------------\n\tHealth: " + 
                iHealth + "\n--------------------------------\n" + iCardText;
        }

        //for heroes
        public void updateDisplay(string iCardName, string iCardText, string iIncludedCards, int iHealth)
        {
            display.text = iCardName + "\n--------------------------------\n\tHealth: " +
                iHealth + "\n--------------------------------\n" + iCardText+ "\n--------------------------------\nAdded Cards: "+iIncludedCards;
        }

        //For standard cards
        public void updateDisplay(string iCardName, string iCardAttack, string iCardBounty, string iCardAttrib) {
            display.text = iCardName + "\n--------------------------------\n" + iCardAttrib+ "\n--------------------------------\nAttack: "
                + iCardAttack + "\n--------------------------------\nBounty: " + iCardBounty;
        }
    }

    //outsideInfo
    public GameObject tilebase;
    public GameObject movingTileBase;
    public GameObject damageIndicatorBase;
    public GameObject bossObject;
    public int bossCode = 0;
    public int heroCode = 0;
    public bool playerTurn = true;

    //Access to other things
    GameObject[,] tiles;
    TileScript[,] tScripts;
    cardInfo[] database;
    bossScript bossCtr;
    TileScript faceupOne;
    TileScript faceupTwo;
    SpriteRenderer sprRenderer;
    ConditionDisplayScr bossConditionScr;
    ConditionDisplayScr heroConditionScr;


    //Access for animation
    GameObject animationObj1;
    GameObject animationObj2;
    animationTileScr aTileScr1;
    animationTileScr aTileScr2;
    DamageIndicatorScr damIndicatScr;

    //Access for UI elements
    TextMeshPro bossHealthDisplay;
    TextMeshPro heroHealthDisplay;

    //Deck Information
    List<string> deckCodes;
    List<string> discardCodes;
    int cardVarieties = 0;

    //For Cmd Codes
    cardInfo lastAtt;
    cardInfo lastBty;
    bool destroyAfter;
    int moveXpos = 0;
    int moveYpos = 0;

    //Game phase handleing:
    bool waitingToDisplay = false;
    int state = 0;
    int nextState = 0;
    int currentlyDisplayed = 0;
    bool alreadyFaceup = false;
    bool midclick = false;

    //Player Variables
    int heroHealth = 30;
    int heroMaxHealth = 30;
    int visibleHeroHealth = 30;
    int heroCond = 0;
    string heroAttCode;
    string heroBtyCode;
    string heroAbilityCode;
    string heroDeckCode;

    //hero metadata and text for the card
    string heroName;
    string heroText;
    string heroDeckText;
    string includedCards;

    //Boss variables
    int bossHealth = 30;
    int bossMaxHealth = 30;
    int visibleBossHealth = 100;
    int bossCond = 0;
    string bossAttCode;
    string bossBtyCode;


    //boss metadata, and text for the card
    string bossName;
    string bossText;

    //Animation Variables
    bool animationComplete = true;
    int runningAnimations = 0;
    string currentAnimation;
    List<string> animationQueue;
    bool revealOnFinish = false;
    int[] toRevealOnFinish = { 0, 0 };

    CardDisplay cardPage;

    bool willDie = false;
    bool willWin = false;




    // Use this for initialization
    void Start() {
        //Initiilize variables for effects.
        lastAtt = new cardInfo("000", "000", "000", "000");
        lastBty = new cardInfo("000", "000", "000", "000");

        //Boss loading
        bossCode = int.Parse(InfoBank.bossNum_s);
        bossCtr = bossObject.GetComponent<bossScript>();
        loadBossComands();

        //Hero loading
        Debug.Log(InfoBank.heroNum_s);
        heroCode = int.Parse(InfoBank.heroNum_s);
        loadHeroComands();
        

        //Load this object, to print out relevant cards when needed.
        sprRenderer = this.GetComponent<SpriteRenderer>();

        //Load effect animations.
        loadAnimations();

        //Deck and field loading. Database stores a copy of every card being used this game, to easily access it.
        database = new cardInfo[11];
        loadDeck();
        popDeck();
        shuffleDeck();
        tiles = new GameObject[4, 3];
        tScripts = new TileScript[4, 3];
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++)
            {
                tiles[i, j] = Instantiate(tilebase, new Vector3(-3 + (i * 2), 0 - (j * 2), 0), Quaternion.identity);
                tScripts[i, j] = tiles[i, j].GetComponent<TileScript>();
                loadTile(i, j);
            }
        }
        animationQueue.Clear();

        cardPage = new CardDisplay("CardDisplay");
    }

    // The Game loop
    void Update()
    {
        switch (state)
        {

            //Making match
            case 0:
                break;

            //When finished displaying, Complete match effect 
            case 1:
                if (!waitingToDisplay)
                {
                    state = 4;
                    nextState = 2;
                    faceupOne.flip();
                    faceupTwo.flip();
                    handleMatch();
                }
                break;
            //choose boss attack
            case 2:
                state = 3;
                StartBossTurn();
                waitingToDisplay = true;
                midclick = true;
                break;

            //When finished displaying, Carry out boss attack.
            case 3:
                if (!waitingToDisplay)
                {
                    state = 4;
                    nextState = 0;
                    resolveBossTurn();
                }
                break;

            //Animations In general. When finished, go to next state. Animation codes are similar to command codes.
            /*Animation Code List: 
             
             
             
             */
            case 4:
                //If finished with animations, go onto next state.

                if (animationQueue.Count == 0 && animationComplete)
                {
                    if (revealOnFinish)
                    {
                        tScripts[toRevealOnFinish[0], toRevealOnFinish[1]].reveal();
                        revealOnFinish = false;
                    }
                    state = nextState;
                } else if (animationComplete) {
                    if (revealOnFinish) {
                        tScripts[toRevealOnFinish[0], toRevealOnFinish[1]].reveal();
                        revealOnFinish = false;
                    }
                    currentAnimation = animationQueue[0];
                    animationQueue.RemoveAt(0);

                    animationComplete = false;
                    startAnimation(currentAnimation);
                    
                }
                //check victory
                if (visibleBossHealth < 1) {
                    state = 6;
                }
                if (visibleHeroHealth < 1) {
                    state = 7;
                }
                break;
                
            //paused Game
            case 5:
                break;

            //Game over win
            case 6:
                SceneManager.LoadScene("WinScene");
                break;

            //Game over lose
            case 7:
                SceneManager.LoadScene("LoseScene");
                break;
        }

        if (Input.GetKeyDown("space"))
        {
            waitingToDisplay = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            waitingToDisplay = false;
        }


    }

    //Callable functions to track interaction
    public void onOtherClick(int cardValue, int x, int y)
    {
        if (state == 0) {
            flipTile(x, y);
            currentlyDisplayed = cardValue;
            if (alreadyFaceup) {
                faceupTwo = tScripts[x, y];
                alreadyFaceup = false;

                playerTurn = false;

                state = 1;
                waitingToDisplay = true;

            } else {
                faceupOne = tScripts[x, y];
                alreadyFaceup = true;
            }
        }
    }
    public void onOtherMouseOver(string iCardName, string iCardAttack, string iCardBounty, string iCardAttrib)
    {
        cardPage.updateDisplay(iCardName,iCardAttack,iCardBounty,iCardAttrib);
    }



    public void onNextPhaseClick() {
        waitingToDisplay = false;
    }
    public void onAnimationFinish() {

        runningAnimations--;

        if (runningAnimations == 0) {
            animationComplete = true;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.richText = true;
        if (waitingToDisplay) {
            GUILayout.Label("<size=30>Push \"Resolve Phase\" or </size>", style);
            GUILayout.Label("<size=30>Right Click to continue.</size>", style);
        }

        
    }

    //TileFunctions
    public void flipTile(int X, int Y) {
        tScripts[X, Y].flip();
    }
    void loadTile(int x, int y) {
        for (int k = 0; k < cardVarieties; k++)
        {
            if (deckCodes.Count < 1) {
                deckCodes.AddRange(discardCodes);
                shuffleDeck();
                discardCodes.Clear();
            }
            if (deckCodes[0] == database[k].getCardID())
            {
                tScripts[x, y].loadTiletoSlot(database[k].getCardID(), database[k].getCardAtr(), database[k].getAttackCode(), 
                    database[k].getBountyCode(), database[k].getCardName(), database[k].getAttackText(), database[k].getBountyText(), 
                    database[k].getFace());
                tScripts[x, y].setLocation(x, y);
                deckCodes.RemoveAt(0);
                animationQueue.Add("N" + x + y);
                break;
            }
        }
    }
    void handleMatch() {
        string aniCode;

        if (faceupOne.getcardID() == faceupTwo.getcardID())
        {
            destroyAfter = false;
            cmdCode(faceupOne.getBountyCode());
            lastBty = new cardInfo(faceupOne);

            cmdCode(bossBtyCode);
            cmdCode(heroBtyCode);

            if (!destroyAfter) {
                discardCodes.Add(faceupOne.getCardID_S());
                discardCodes.Add(faceupOne.getCardID_S());
                aniCode = "M" + faceupOne.getXLoc() + faceupOne.getYLoc() + faceupTwo.getXLoc() + faceupTwo.getYLoc();
                Debug.Log("Anicode: " + aniCode);
                animationQueue.Add(aniCode);
            }
            loadTile(faceupOne.getXLoc(), faceupOne.getYLoc());
            loadTile(faceupTwo.getXLoc(), faceupTwo.getYLoc());
        }
    }

    //Deck management functions
    void loadDeck()
    {
        //Initialize the decks
        deckCodes = new List<string>();
        discardCodes = new List<string>();

        //Load The deck
        TextAsset deckList = Resources.Load("TextAssets/deckList", typeof(TextAsset)) as TextAsset;
        string bossDeck;


        string[] decks = Regex.Split(deckList.text, "\n");
        string[] heroCont = Regex.Split(heroDeckCode, ";");

        bossDeck = decks[bossCode];
        bossDeck = bossDeck.Remove(0, 3);

        string[] cards = Regex.Split(bossDeck, ";"); 
        string cardCode_s;
        string numOf_s;
        int numOf;

        for (int i = 0; i < cards.Length; i++) {

            cardCode_s = cards[i].Substring(0, 3);
            numOf_s = cards[i].Substring(3, 2);
            numOf = int.Parse(numOf_s);
            database[cardVarieties] = new cardInfo(cardCode_s);
            cardVarieties++;
            if (numOf > 0)
            {
                for (int j = 0; j < numOf; j++)
                {
                    deckCodes.Add(cardCode_s);

                }
            }
        }
        for (int i = 0; i < heroCont.Length; i++)
        {

            cardCode_s = heroCont[i].Substring(0, 3);
            numOf_s = heroCont[i].Substring(3, 2);
            numOf = int.Parse(numOf_s);
            database[cardVarieties] = new cardInfo(cardCode_s);
            cardVarieties++;
            if (numOf > 0)
            {
                for (int j = 0; j < numOf; j++)
                {
                    deckCodes.Add(cardCode_s);

                }
            }
        }



    }
    void popDeck()
    {
        TextAsset cardList = Resources.Load("TextAssets/cardCode", typeof(TextAsset)) as TextAsset;
        string cardList_s = cardList.text;
        string[] cardInfo = Regex.Split(cardList_s, "\n|\r|\r\n");

        //needed infomation to pop cardlist


        string[] currentLine;

        for (int i = 0; i < cardVarieties; i++) {
            for (int j = 1; j < cardInfo.Length; j++) {
                currentLine = Regex.Split(cardInfo[j], "!");
                if (currentLine[0] == database[i].getCardID()) {
                    database[i] = new cardInfo(currentLine[0], currentLine[1], currentLine[3], currentLine[5], currentLine[2], currentLine[4], currentLine[6]);
                    break;
                }
            }
        }

    }
    void shuffleDeck() {
        string temp;
        int otherLocation;
        for (int i = 0; i < deckCodes.Count; i++) {
            otherLocation = Random.Range(0, deckCodes.Count);
            temp = deckCodes[otherLocation];
            deckCodes[otherLocation] = deckCodes[i];
            deckCodes[i] = temp;
        }
    }

    //Hero Functions
    void loadHeroComands()
    {
        TextAsset heroInfo = Resources.Load("TextAssets/heroData", typeof(TextAsset)) as TextAsset;
        string[] heroInfoList = Regex.Split(heroInfo.text, "\n");

        for (int i = 1; i < heroInfoList.Length; i++)
        {
            Debug.Log(heroInfoList[i]);
            string[] bossInfoBreakdown = Regex.Split(heroInfoList[i], "!");

            if (int.Parse(bossInfoBreakdown[0]) == heroCode)
            {
                heroName = bossInfoBreakdown[1];
                heroHealth = int.Parse(bossInfoBreakdown[2]);
                heroMaxHealth = int.Parse(bossInfoBreakdown[2]);
                heroText = bossInfoBreakdown[3];
                heroDeckText = bossInfoBreakdown[4];
                heroAttCode = bossInfoBreakdown[5];
                heroBtyCode = bossInfoBreakdown[6];
                heroAbilityCode = bossInfoBreakdown[7];
                heroDeckCode = bossInfoBreakdown[8];
                break;
            }
        }
        visibleHeroHealth = heroMaxHealth;
        heroHealthDisplay = GameObject.Find("HeroHealthText").GetComponent<TextMeshPro>();
        heroHealthDisplay.text = heroMaxHealth + "/" + heroMaxHealth;

        heroConditionScr = GameObject.Find("HeroCond").GetComponent<ConditionDisplayScr>();
    }
    public void onHeroMouseOver() {
        cardPage.updateDisplay(heroName, heroText, heroDeckText, heroMaxHealth);
    }

    //Boss Functions
    void StartBossTurn() {
        moveXpos = Random.Range(0, 4);
        moveYpos = Random.Range(0, 3);
        tScripts[moveXpos, moveYpos].flip();

    }
    void resolveBossTurn() {
        flipTile(moveXpos, moveYpos);
        cmdCode(tScripts[moveXpos, moveYpos].getAttackCode());

        lastAtt = new cardInfo(tScripts[moveXpos, moveYpos]);
        cmdCode(bossAttCode);
        cmdCode(heroAttCode);
        
        
        //Resolve end of turn effects of conditions
        switch (bossCond)
        {
            case 2:{
                    cmdCode("D001");
                break;
            }
            case 3: {
                    cmdCode("R001");
                break;
            }
        }

        switch (heroCond) {
            case 2:
                {
                    cmdCode("S001");
                    break;
                }
            case 3:
                {
                    cmdCode("H001");
                    break;
                }
        }
    }
    void loadBossComands() {
        TextAsset bossInfo = Resources.Load("TextAssets/bossData", typeof(TextAsset)) as TextAsset;
        string[] bossInfoList = Regex.Split(bossInfo.text, "\n");

        for (int i = 1; i < bossInfoList.Length; i++) {
            string[] bossInfoBreakdown = Regex.Split(bossInfoList[i], "!");
            
            if (int.Parse(bossInfoBreakdown[0]) == bossCode) {
                bossHealth = int.Parse(bossInfoBreakdown[2]);
                bossMaxHealth = int.Parse(bossInfoBreakdown[2]);
                bossName = bossInfoBreakdown[1];
                bossText = bossInfoBreakdown[3];
                bossAttCode = bossInfoBreakdown[4];
                bossBtyCode = bossInfoBreakdown[5];
                visibleBossHealth = bossMaxHealth;
                break;
            }
        }
        bossCtr.setAllBossInfo(bossAttCode, bossBtyCode);
        bossHealthDisplay = GameObject.Find("BossHealthText").GetComponent<TextMeshPro>();
        bossHealthDisplay.text = bossMaxHealth + "/" + bossMaxHealth;

        //Load condition display
        bossConditionScr = GameObject.Find("BossCond").GetComponent<ConditionDisplayScr>();
    }
    public void onBossMouseOver() {
        cardPage.updateDisplay(bossName, bossText, bossMaxHealth);
    }

    //Animation Functions
    /* Animation Codes:
     * 
     * D: (D)iscard a card. Location is the next two characters, from 0 to 4. (Ex. D00-D43)
     * N: (N)ew card from deck. Location is next two characters, from 0 to 4.
     * S: Used to denote a (S)pecial animation such as dust, or cloud effects.
     * M: Complete (M)atch animation. Locations are next 4 characters. (Ex. D0000-D4343)
     * H: Shuffle the card into the deck
     * R: Remove from play
     * T: Show hero (T)ake damage
     * A: Show Boss T(A)ke damage
     * L: Heal from hero
     * E: Heal from boss
    */

    void loadAnimations() {
        animationQueue = new List<string>();
    }
    void startAnimation(string animationCode) {
        int xOne, xTwo, yOne, yTwo, damage;
        char[] brokenUp = animationCode.ToCharArray();
        string health;


        switch (brokenUp[0]) {
            case 'D':
                {
                    xOne = int.Parse(brokenUp[1].ToString());
                    yOne = int.Parse(brokenUp[2].ToString());
                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(movingTileBase, new Vector3(-3 + (xOne * 2), 0 - (yOne * 2), -1), Quaternion.identity);
                    aTileScr1 = animationObj1.GetComponent<animationTileScr>();
                    aTileScr1.initTile(.65, 2);
                    tScripts[xOne, yOne].hide();
                    runningAnimations = 1;
                    break;
                }
            case 'M':
                {
                    //Get the rest of the data

                    xOne = int.Parse(brokenUp[1].ToString());
                    yOne = int.Parse(brokenUp[2].ToString());
                    xTwo = int.Parse(brokenUp[3].ToString());
                    yTwo = int.Parse(brokenUp[4].ToString());

                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(movingTileBase, new Vector3(-3 + (xOne * 2), 0 - (yOne * 2), -1), Quaternion.identity);
                    aTileScr1 = animationObj1.GetComponent<animationTileScr>();
                    aTileScr1.initTile(.65, 2);
                    tScripts[xOne, yOne].hide();

                    animationObj2 = new GameObject();
                    animationObj2 = Instantiate(movingTileBase, new Vector3(-3 + (xTwo * 2), 0 - (yTwo * 2), -1), Quaternion.identity);
                    aTileScr2 = animationObj2.GetComponent<animationTileScr>();
                    aTileScr2.initTile(.65, 2);

                    tScripts[xTwo, yTwo].hide();
                    runningAnimations = 2;

                    break;
                }
            case 'N':
                {
                    xOne = int.Parse(brokenUp[1].ToString());
                    yOne = int.Parse(brokenUp[2].ToString());
                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(movingTileBase, new Vector3(-1.5f, 2, -1), Quaternion.identity);
                    aTileScr1 = animationObj1.GetComponent<animationTileScr>();
                    aTileScr1.initTile(-3 + (xOne * 2), 0 - (yOne * 2));
                    revealOnFinish = true;
                    toRevealOnFinish[0] = xOne;
                    toRevealOnFinish[1] = yOne;
                    runningAnimations = 1;
                    break;
                }
            case 'H':
                {
                    xOne = int.Parse(brokenUp[1].ToString());
                    yOne = int.Parse(brokenUp[2].ToString());
                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(movingTileBase, new Vector3(-3 + (xOne * 2), 0 - (yOne * 2), -1), Quaternion.identity);
                    aTileScr1 = animationObj1.GetComponent<animationTileScr>();
                    aTileScr1.initTile(-1.5f, 2);
                    tScripts[xOne, yOne].hide();
                    runningAnimations = 1;
                    break;
                }
            case 'R':
                {
                    xOne = int.Parse(brokenUp[1].ToString());
                    yOne = int.Parse(brokenUp[2].ToString());
                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(movingTileBase, new Vector3(-3 + (xOne * 2), 0 - (yOne * 2), -1), Quaternion.identity);
                    aTileScr1 = animationObj1.GetComponent<animationTileScr>();
                    aTileScr1.initTile(0, -6);
                    tScripts[xOne, yOne].hide();
                    runningAnimations = 1;
                    break;
                }
            //The summoners of the damage Indicator
            case 'T':
                {
                    health = animationCode.Substring(1, 3);

                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(damageIndicatorBase, new Vector3(-5.25f, -3, -1), Quaternion.identity);

                    damIndicatScr = animationObj1.GetComponent<DamageIndicatorScr>();
                    damIndicatScr.initText("-" + int.Parse(health).ToString(), new Color32(255, 0, 0, 255));

                    runningAnimations = 1;
                    visibleHeroHealth -= int.Parse(health);
                    break;
                }
            case 'A':
                {
                    health = animationCode.Substring(1, 3);

                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(damageIndicatorBase, new Vector3(6.25f, 3, -1), Quaternion.identity);

                    damIndicatScr = animationObj1.GetComponent<DamageIndicatorScr>();
                    damIndicatScr.initText("-" + int.Parse(health).ToString(), new Color32(255, 0, 0, 255));

                    runningAnimations = 1;
                    visibleBossHealth -= int.Parse(health);
                    break;
                }
            case 'L':
                {

                    health = animationCode.Substring(1, 3);

                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(damageIndicatorBase, new Vector3(-5.25f, -3, -1), Quaternion.identity);

                    damIndicatScr = animationObj1.GetComponent<DamageIndicatorScr>();
                    damIndicatScr.initText("+" + int.Parse(health).ToString(), new Color32(0, 255, 0, 255));

                    runningAnimations = 1;
                    visibleHeroHealth += int.Parse(health);
                    break;
                }
            case 'E':
                {
                    health = animationCode.Substring(1, 3);

                    animationObj1 = new GameObject();
                    animationObj1 = Instantiate(damageIndicatorBase, new Vector3(6.25f, 3, -1), Quaternion.identity);

                    damIndicatScr = animationObj1.GetComponent<DamageIndicatorScr>();
                    damIndicatScr.initText("+" + int.Parse(health).ToString(), new Color32(0, 255, 0, 255));

                    runningAnimations = 1;
                    visibleBossHealth += int.Parse(health);
                    break;
                }

        }
        updateHealthBars();
    }
    void updateHealthBars() {
        if (visibleBossHealth > bossMaxHealth) {
            visibleBossHealth = bossMaxHealth;
        }
        if (visibleHeroHealth > heroMaxHealth) {
            visibleHeroHealth = heroMaxHealth;
        }
        bossHealthDisplay.text = visibleBossHealth + "/" + bossMaxHealth;
        heroHealthDisplay.text = visibleHeroHealth + "/" + heroMaxHealth;
    }

    /* The bit that uses all of the codes that the system uses.
     code meanings:

    Health manipulation:
        (S)trike                Deal damage to player
        (D)eal                  Deal Damage to boss
        (H)eal	                Heal from player (Number is Health Restored)
        (R)egenerate	        Heal from boss (Number is Health Restored)


    Card manipulation:
        shu(F)fle               Shuffle the attacking card into the deck.
        remo(V)e                Remove both matched cards from the game.
        destro(Y)               Remove the attacking card from play.
        (A)dd Card              Shuffle new copy of stated card into deck.

    Basic effects:
        r(E)distribute          Shuffle all cards into the deck, then deal out 12
        (C)ondition             Add condition to hero.
        c(O)ndition             Add condition to boss.
        re(P)eat last match     Repeats last match effect of a matched card
        
        Other:
        (N)ull effect           Do nothing;

        
    Conditionals(Try to keep lowercase)
        (a)ttribute is          Check attribute of last card that ATTACKED. if It matches command code, continue.
        a(t)tribute is          check attrubute of last MATCHED card.
        (p)hase is              Check current phase. 
        (l)ast attack card is   Check last attack card ID. If it matches code, continue
        la(s)t bounty card is   Check last bounty card ID. If it matches code, continue.
    
    
    
    
    To add:
        

    */
    public void cmdCode(string effectCode) {
        Debug.Log(effectCode);
        //For math related to health changes
        int healthMod = 0;

        string[] allCommands = Regex.Split(effectCode, ";");
        foreach (string command in allCommands) {
            char[] brokenUp = command.ToCharArray();
            char effectType = brokenUp[0];
            string number = command.Substring(1, 3);
            
            switch (effectType)
            {

                //Health Manupulators
                case 'S':{
                        heroHealth -= int.Parse(number);
                        animationQueue.Add("T" + number);
                        break;
                    }
                case 'D':{
                        if (bossCond != 1)
                        {
                            bossHealth -= int.Parse(number);
                            animationQueue.Add("A" + number);
                        }
                        break;
                    }
                case 'H':{
                        heroHealth += int.Parse(number);
                        animationQueue.Add("L" + number);
                        break;
                    }
                case 'R':{
                        bossHealth += int.Parse(number);
                        animationQueue.Add("E" + number);
                        break;
                    }

                //Card manipulation
                //Shuffle the attacking card into the deck.
                case 'F':{
                        animationQueue.Add("H" + moveXpos + moveYpos);
                        deckCodes.Add(tScripts[moveXpos, moveYpos].getCardID_S());
                        shuffleDeck();
                        loadTile(moveXpos, moveYpos);
                        break;
                    }

                //Destroy the matched cards.
                case 'V':{
                        destroyAfter = true;
                        break;
                    }

                //Remove the attacking card from play.
                case 'Y':{
                        animationQueue.Add("R" + moveXpos + moveYpos);
                        loadTile(moveXpos, moveYpos);
                        break;
                    }

                //Discard Attacking Card.
                case 'B':{
                        discardCodes.Add(lastAtt.getCardID());
                        animationQueue.Add("D" + moveXpos+moveYpos);
                        loadTile(moveXpos, moveYpos);
                        break;
                }

                //Add a new card to the deck.
                case 'A':{
                        deckCodes.Add(number);
                        shuffleDeck();
                        break;
                    }

                //Other
                //Shuffle all into deck, then deal them back out.
                case 'E':{
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                deckCodes.Add(tScripts[i, j].getCardID_S());
                                animationQueue.Add("H" + i + j);
                            }
                        }
                        shuffleDeck();
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {

                                loadTile(i, j);
                            }
                        }
                        break;
                    }

                //Repeat the Bounty Code
                case 'P':{
                        cmdCode(lastBty.getBountyCode());
                        break;
                }

                //Condition Management
                case 'O':{
                        if (bossCond != 1)
                        {
                            bossCond = int.Parse(number);
                            bossConditionScr.updateCondition(int.Parse(number));
                        }
                        break;
                    }
                case 'C':{
                        heroCond = int.Parse(number);
                        heroConditionScr.updateCondition(int.Parse(number));
                        break;
                    }
                case 'X':{
                        if (bossCond == 1) {
                            bossCond = 0;
                            bossConditionScr.updateCondition(0);
                        }
                    break;
                }

                //Null Case
                case 'N':{ break; }
                    
                //Conditionals:
                //Check last attack Atribute
                case 'a':{
                        if (command.Length > 8)
                        {
                            if (lastAtt.getCardAtr() == number)
                            {
                                cmdCode(command.Remove(0, 5));
                            }
                        }
                        break;
                    }

                //Check last bounty attribute
                case 't':{
                        if (command.Length > 8)
                        {
                            if (lastBty.getCardAtr() == number)
                            {
                                cmdCode(command.Remove(0, 5));
                            }
                        }
                        break;
                    }

                //Last attack card ID
                case 'l':{
                        
if (command.Length > 8)
                        {
                            if (lastAtt.getCardID() == number)
                            {
                                cmdCode(command.Remove(0, 5));
                            }
                        }
                        break;
                    }

                //check last bounty ID
                case 's':{
                        if (command.Length > 8)
                        {
                            if (lastBty.getCardID() == number)
                            {
                                cmdCode(command.Remove(0, 5));
                            }
                        }
                        break;
                    }

                //check state. 
                case 'p':{
                        if (command.Length > 8)
                        {
                            if (state == int.Parse(number))
                            {
                                cmdCode(command.Remove(0, 5));
                            }
                        }
                        break;
                    }
            }

        }

        //fix health
        if (heroHealth > heroMaxHealth) {
            heroHealth = heroMaxHealth;
        }
        if (bossHealth > bossMaxHealth) {
            bossHealth = bossMaxHealth;
        }
    }
}
