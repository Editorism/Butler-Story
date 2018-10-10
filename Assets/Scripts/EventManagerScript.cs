using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManagerScript : MonoBehaviour {

    public const string cardspath = "Cards";
    public const string optionspath = "Options";

    public string playerName = "";

    public static CardContainer cContainer;

    public static OptionContainer oContainer;

    public GameObject CharacterStart;
    public GameObject CharacterPosition;
    public GameObject MasterSprite;
    public GameObject GrandmotherSprite;
    public GameObject HarrySprite;
    public GameObject CookSprite;
    public GameObject MaidSprite;
    public GameObject GroundskeeperSprite;
    public GameObject ClockHand;
    public GameObject ClockMorning;
    public GameObject ClockMidday;
    public GameObject ClockAfternoon;
    public GameObject ClockEvening;
    public GameObject Menu_UI;
    public GameObject Game_UI;
    public GameObject Victory_UI;
    public GameObject Defeat_UI;

    public Button optionButton1;
    public Button optionButton2;
    public Button optionButton3;

    public Slider masterSlider;
    public Slider staffSlider;
    public Slider moneySlider;

    public Text eventText;
    public Text eventCauserTxt;
    public Text dayCounterTxt;

    private int currentCardIndex;
    private int dayCounter = 1;
    private int timeOfDay = 0;
    private int lastTimeOfDay;

    private Quaternion currentRot;
    private Quaternion newRot;

    private GameObject currentSprite;
    private GameObject shownSprite;
    private GameObject lastSprite;

    private float masterOpinion;
    private float staffOpinion;
    private float moneyLevel;

    private float duration = 1.0f;
    private float startTime;
    private float t;
    private float clockStart;
    private float clockT;

    private static bool cardDrawn = false;
    private bool CharacterLerped = false;
    private bool Lerping = true;
    private bool CharacterHidden = true;
    private bool gameWon = false;
    private bool gameLost = false;
    private bool gamePaused = false;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level_01")
        {
            cardDrawn = false;
            masterOpinion = masterSlider.maxValue;
            staffOpinion = staffSlider.maxValue;
            moneyLevel = moneySlider.maxValue;
            
        }
        playerName = PlayerPrefs.GetString("PlayerName");
        LoadCardStream(cardspath);
        LoadOptionStream(optionspath);
    }

    // Update is called once per frame
    void Update()
    {
      
        if (SceneManager.GetActiveScene().name == "Level_01")
        {
            
            TimeOfDayChecker();

            EscMenuChecker();

            GameStateChecker();

            t = (Time.time - startTime) / duration;
            clockT = (Time.time - clockStart) / duration;


            ClockHand.transform.rotation = Quaternion.Slerp(ClockHand.transform.rotation, newRot, clockT);
            if(ClockHand.transform.rotation.z <= newRot.z + 1 && ClockHand.transform.rotation.z >= newRot.z - 1)
            {
                currentRot = newRot;
            }

            SliderChecker();

            if (cardDrawn == false)
            {

                DrawCard();
                cardDrawn = true;
            }

            if(Lerping == true)
        {
            optionButton1.interactable = false;
            optionButton2.interactable = false;
            optionButton3.interactable = false;
        }
        else
        {
            optionButton1.interactable = true;
            optionButton2.interactable = true;
            optionButton3.interactable = true;
        }
            if (lastSprite != currentSprite)
            {
                SpriteMover();
            }

            if (cContainer.cards.Count == 0)
            {

                ShuffleDecks();
            }
        }
    }	



    public void GiveAnswer(int buttonIndex)
    {
        startTime = Time.time;
        for (int i = 0; i < oContainer.options.Count; i++)
        {
           
            Option temp = oContainer.options[i];
            if(buttonIndex == temp.optionIndex && temp.cardIndex == currentCardIndex)
            {
                lastSprite = currentSprite;
                staffOpinion += temp.staffEffect;
                masterOpinion += temp.mastersEffect;
                lastTimeOfDay = timeOfDay;
                timeOfDay += temp.timeEffect;
                TimeChecker(temp.timeEffect);
                moneyLevel += temp.moneyEffect;
            }
        }
        cardDrawn = false;
    }

    public void DrawCard()
    {
        int randomNumber = Random.Range(0, cContainer.cards.Count);

        Card randomCard = cContainer.cards[randomNumber];

        if(randomCard.spriteName == "MasterSprite")
        {
            currentSprite = MasterSprite;
        }
        else if(randomCard.spriteName == "GrandmotherSprite")
        {
            currentSprite = GrandmotherSprite;
        }
        else if (randomCard.spriteName == "HarrySprite")
        {
            currentSprite = HarrySprite;
        }
        else if (randomCard.spriteName == "CookSprite")
        {
            currentSprite = CookSprite;
        }
        else if (randomCard.spriteName == "MaidSprite")
        {
            currentSprite = MaidSprite;
        }
        else if (randomCard.spriteName == "GroundskeeperSprite")
        {
            currentSprite = GroundskeeperSprite;
        }
        else
        {
            currentSprite = shownSprite;
        }
        

        string cardText = randomCard.eventText;
    
        string pattern = "PlayerName";

        string result = cardText.Replace(pattern, playerName);

        eventCauserTxt.text = randomCard.eventCauser + ":";

        for (int i = 0; i < oContainer.options.Count; i++)
        {

            if (randomCard.cardIndex == oContainer.options[i].cardIndex)
            {
                if(oContainer.options[i].optionIndex == 1)
                {
                    optionButton1.GetComponentInChildren<Text>().text = oContainer.options[i].optionText;
                }
                else if (oContainer.options[i].optionIndex == 2)
                {
                    optionButton2.GetComponentInChildren<Text>().text = oContainer.options[i].optionText;
                }
                else
                {
                    optionButton3.GetComponentInChildren<Text>().text = oContainer.options[i].optionText;
                }


            }
        }
        currentCardIndex = randomCard.cardIndex;

        cContainer.cards.Remove(randomCard);

        cContainer.discardPile.Add(randomCard);

        eventText.text = result;

        SaveCardStream(cardspath);
    }

    public void ShuffleDecks()
    {
        for(int i = 0; i< cContainer.discardPile.Count; i++)
        {
            Card temp = cContainer.discardPile[i];

            cContainer.discardPile.Remove(temp);

            cContainer.cards.Add(temp);

            SaveCardStream(cardspath);
        }

    }

    void SpriteMover()
    {
         
            if (CharacterLerped == true && CharacterHidden == false)
            {
                shownSprite.transform.position = new Vector3(Mathf.SmoothStep(CharacterPosition.transform.position.x, CharacterStart.transform.position.x, t), CharacterPosition.transform.position.y, 0);
                if (shownSprite.transform.position.x >= CharacterStart.transform.position.x - 1 && shownSprite.transform.position.x <= CharacterStart.transform.position.x + 1)
                {
                    CharacterHidden = true;
                    CharacterLerped = false;
                }
            }

        
            currentSprite.transform.position = new Vector3(Mathf.SmoothStep(CharacterStart.transform.position.x, CharacterPosition.transform.position.x, t), CharacterPosition.transform.position.y, 0);
            if (currentSprite.transform.position.x <= CharacterPosition.transform.position.x + 1 && currentSprite.transform.position.x >= CharacterPosition.transform.position.x - 1)
            {
                Lerping = false;
                CharacterLerped = true;
                CharacterHidden = false;
                shownSprite = currentSprite;
            }
            else
            {
                Lerping = true;
            }
    }

    void SliderChecker()
    {
        if (staffOpinion <= staffSlider.maxValue)
        {
            staffSlider.value = Mathf.SmoothStep(staffSlider.value, staffOpinion, t);
        }
        else
        {
            staffOpinion = staffSlider.maxValue;
        }

        if (masterOpinion <= masterSlider.maxValue)
        {
            masterSlider.value = Mathf.SmoothStep(masterSlider.value, masterOpinion, t);
        }
        else
        {
            masterOpinion = masterSlider.maxValue;
        }

        if (moneyLevel <= moneySlider.maxValue)
        {
            moneySlider.value = Mathf.SmoothStep(moneySlider.value, moneyLevel, t);
        }
        else
        {
            moneyLevel = moneySlider.maxValue;
        }
    }

    void EscMenuChecker()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (gamePaused == true)
            {
                Game_UI.SetActive(true);
                Menu_UI.SetActive(false);
                gamePaused = false;
            }
            else
            {
                Game_UI.SetActive(false);
                Menu_UI.SetActive(true);
                gamePaused = true;
            }
        }
    }
    
    void GameStateChecker()
    {
        dayCounterTxt.text = dayCounter.ToString();
        if (gameWon == true)
        {
            Victory_UI.SetActive(true);
            Game_UI.SetActive(false);
        }

        if (dayCounter >= 4)
        {
            gameWon = true;
        }

            if (timeOfDay >= 12)
            {
                if (lastTimeOfDay == 11)
                {
                    timeOfDay = 1;
                }
                else
                {
                    timeOfDay = 0;
                }
                    dayCounter++;
            }

        if (gameLost == true)
        {
            Defeat_UI.SetActive(true);
            Game_UI.SetActive(false);
        }
        if (staffSlider.value <= 0 || masterSlider.value <= 0 || moneySlider.value <= 0)
        {
            gameLost = true;
        }
    }

    void TimeOfDayChecker()
    {
        if(timeOfDay >= 0 && timeOfDay < 3)
        {
            ClockMorning.SetActive(true);
            ClockMidday.SetActive(false);
            ClockAfternoon.SetActive(false);
            ClockEvening.SetActive(false);
        }
        else if (timeOfDay >= 3 && timeOfDay < 6)
        {
            ClockMorning.SetActive(false);
            ClockMidday.SetActive(true);
            ClockAfternoon.SetActive(false);
            ClockEvening.SetActive(false);
        }
        else if (timeOfDay >= 6 && timeOfDay < 9)
        {
            ClockMorning.SetActive(false);
            ClockMidday.SetActive(false);
            ClockAfternoon.SetActive(true);
            ClockEvening.SetActive(false);
        }
        else
        {
            ClockMorning.SetActive(false);
            ClockMidday.SetActive(false);
            ClockAfternoon.SetActive(false);
            ClockEvening.SetActive(true);
        }
    }

    void TimeChecker(int timeEffect)
    {
        clockStart = Time.time;
        currentRot = ClockHand.transform.rotation;
        if(newRot.z <= 360 + 1 && newRot.z >= 360 - 1)
        {
            newRot.z = 0;
        }
        newRot = Quaternion.Euler(currentRot.eulerAngles.x,currentRot.eulerAngles.y,(-360/12* timeOfDay));
        if(ClockHand.transform.rotation.z <= 360 + 0.01f && ClockHand.transform.rotation.z >= 360 - 0.01f)
        {
            currentRot.z = 0;
        }
    }
    public void LoadMenu()
    {   
        SceneManager.LoadScene("Main_Menu");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Level_01");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeName(string newName)
    {

        PlayerPrefs.SetString("PlayerName", newName);

    }

    public static void LoadCardStream(string path)
    {

        cContainer = CardContainer.Load(path);
    }

    public static void LoadOptionStream(string path)
    {

        oContainer = OptionContainer.Load(path);
    }

    public static void SaveCardStream(string path)
    {

        cContainer.Save(path);
    }

    public static void SaveOptionStream(string path)
    {

        oContainer.Save(path);
    }
}
