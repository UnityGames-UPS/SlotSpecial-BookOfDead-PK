using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class GambleController : MonoBehaviour
{
    [SerializeField] private SocketIOManager socketManager; // Reference to the SocketIO Manager
    [SerializeField] private AudioController audioController; // Reference to the Audio Controller
    [Space]
    [Header("UI and Components")]
    [SerializeField] private GameObject gamble_game; // The main gamble game object
    [SerializeField] private SlotBehaviour slotController; // Reference to the Slot Controller
    [SerializeField] private Sprite[] HeartSpriteList; // List of heart suit sprites
    [SerializeField] private Sprite[] ClubSpriteList; // List of club suit sprites
    [SerializeField] private Sprite[] SpadeSpriteList; // List of spade suit sprites
    [SerializeField] private Sprite[] DiamondSpriteList; // List of diamond suit sprites
    [SerializeField] private Sprite cardCover; // Default card cover sprite
    [SerializeField] private CardFlip DealerCard_Script; // Reference to the dealer's card flip script

    [Space]
    [Header("History")]
    [SerializeField] private List<Image> history;
    [SerializeField] private List<Sprite> cardIndi;
    private int HistoryCount = 0;



    [Space]
    [Header("User Input")]
    [SerializeField] private Button doubleButton; // Button for starting gameble game
    [SerializeField] private Button RedBtn; // Button for starting gameble game
    [SerializeField] private Button BlackBtn; // Button for starting gameble game
    [SerializeField] private Button SpadeBtn; // Button for starting gameble game
    [SerializeField] private Button JackBtn; // Button for starting gameble game
    [SerializeField] private Button HeartBtn; // Button for starting gameble game
    [SerializeField] private Button DiamondBtn; // Button for starting gameble game

    [Space]
    [Header("TExts")]
    [SerializeField] private TMP_Text ColourWin; // Text to display the win amount
    [SerializeField] private TMP_Text SuitWin; // Text to display the win amount
    [SerializeField] private TMP_Text winamount; // Text to display the win amount
    [SerializeField] private TMP_Text TopText; // Text to display the win amount

    [Space(20)]
    // UI and Components References
    [Header("UI and Components")]
    [SerializeField] internal List<CardFlip> allcards = new List<CardFlip>(); // List of all card flip objects

    // Gamble Section References
    [Header("Gamble Section References")]
    [SerializeField] private GameObject GambleEnd_Object; // Object to display when gamble ends
    [SerializeField] private Button m_Collect_Button; // Button for collecting winnings
    [SerializeField] private Button m_Double_Button; // Button for doubling winnings


    // Internal Variables
    private Sprite highcard_Sprite; // Sprite for the high card
    private Sprite lowcard_Sprite; // Sprite for the low card
    private Sprite spare1card_Sprite; // Sprite for the first spare card
    private Sprite spare2card_Sprite; // Sprite for the second spare card
    internal bool gambleStart = false; // Indicates if the gamble has started

    private bool isAutoSpinOn;
    private string[] cardSuits = new string[] { "Hearts", "Diamonds", "Clubs", "Spades" };

    private Tweener Gamble_Tween_Scale = null; // Tweener for scaling the double button

    #region Initialization

    private void Start()
    {
        // Setup event listeners for buttons
        if (doubleButton)
        {
            doubleButton.onClick.RemoveAllListeners();
            doubleButton.onClick.AddListener(delegate { StartGamblegame(); });
        }

        // Collect Button Setup
        if (m_Collect_Button)
        {
            m_Collect_Button.onClick.RemoveAllListeners();
            m_Collect_Button.onClick.AddListener(() => { slotController.GambleCollect(); gamble_game.SetActive(false); });
        }

        // //Double Button Setup
        // if (m_Double_Button)
        // {
        //     m_Double_Button.onClick.RemoveAllListeners();
        //     m_Double_Button.onClick.AddListener(delegate { NormalCollectFunction(); StartGamblegame(true); });
        // }

        RedBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("RED")); });
        BlackBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("BLACK")); });
        SpadeBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("SPADES")); });
        HeartBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("HEARTS")); });
        DiamondBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("DIAMONDS")); });
        JackBtn.onClick.AddListener(delegate { StartCoroutine(OnClickButtons("CLUBS")); });



        toggleDoubleButton(false); // Disable double button at start
    }

    #endregion

    #region Button Toggle

    // Toggles the interactability of the double button
    internal void toggleDoubleButton(bool toggle)
    {
        doubleButton.interactable = toggle;
    }
    internal void toggleAllButton(bool toggle)
    {
        RedBtn.interactable = toggle;
        BlackBtn.interactable = toggle;
        SpadeBtn.interactable = toggle;
        HeartBtn.interactable = toggle;
        DiamondBtn.interactable = toggle;
        JackBtn.interactable = toggle;
    }

    #endregion

    #region Gamble Game

    // Starts the gamble game
    void StartGamblegame()
    {
        //  isOut = false;
        //    if (GambleEnd_Object) GambleEnd_Object.SetActive(false); // Hide end screen

        //   if (!isRepeat)
        isAutoSpinOn = slotController.IsAutoSpin;

        GambleTweeningAnim(false); // Stop animation
        slotController.DeactivateGamble(); // Deactivate the gamble slot
                                           // winamount.text = "0"; // Reset win amount text

        //  if (!isRepeat) winamount.text = "0"; // Reset win amount on non-repeat

        AllCardToggle(false);
        if (audioController) audioController.PlayButtonAudio(); // Play button click audio
        if (gamble_game) gamble_game.SetActive(true); // Activate gamble game object
        socketManager.isResultdone = false;
        socketManager.OnGamble();


        AllCardToggle(true);
        // StartCoroutine(loadingRoutine()); // Start loading routine
        // StartCoroutine(GambleCoroutine(isRepeat)); // Start gamble coroutine
    }

    // Resets the game and collects winnings


    IEnumerator OnClickButtons(string types)
    {
        if (audioController) audioController.PlayButtonAudio();
        toggleAllButton(false);
        socketManager.isResultdone = false;
        socketManager.GambleDraw(types);

        yield return new WaitUntil(() => socketManager.isResultdone);


        if (DealerCard_Script)
        {
            string valueStr = socketManager.GambleData.payload.card.value;
            int cardValue;

            if (valueStr == "A" || valueStr == "K" || valueStr == "Q" || valueStr == "J")
                cardValue = 10;
            else if (!int.TryParse(valueStr, out cardValue))
                cardValue = 10; // Default fallback if parsing fails

            DealerCard_Script.FlipMyObject(CardSet(socketManager.GambleData.payload.card.suit, cardValue));
        }
        SetHistory(socketManager.GambleData.payload.card.suit);
        if (socketManager.GambleData.payload.playerWon)
        {
            if (audioController) audioController.PlayWLAudio("win");
            winamount.text = "You Won " + socketManager.GambleData.payload.winAmount.ToString();
            ColourWin.text = (socketManager.GambleData.payload.winAmount * 2).ToString();
            SuitWin.text = (socketManager.GambleData.payload.winAmount * 4).ToString();
        }
        else
        {
            ColourWin.text = 0.ToString();
            SuitWin.text = "0";
            winamount.text = "You Loose";

        }
        yield return new WaitForSeconds(2f);

        if (DealerCard_Script) DealerCard_Script.FlipMyObject(cardCover);



        if (socketManager.GambleData.payload.playerWon)
        {
            toggleAllButton(true);
        }
        else
        {
            socketManager.OnCollect();
            slotController.updateBalance();
            if (gamble_game) gamble_game.SetActive(false);
            if (isAutoSpinOn)
            {


                slotController.AutoSpin();
            }
        }


    }

    #endregion



    private void SetHistory(string suit)
    {
        Sprite tempSprite = null;
        switch (suit.ToUpper())
        {
            case "HEARTS":
                tempSprite = cardIndi[0];
                break;
            case "DIAMONDS":
                tempSprite = cardIndi[1];
                break;
            case "CLUBS":
                tempSprite = cardIndi[2];
                break;
            case "SPADES":
                tempSprite = cardIndi[3];
                break;
            default:
                Debug.LogError("Invalid Suit: " + suit);
                break;


        }

        if (HistoryCount < history.Count)
        {
            HistoryCount++;
            history[HistoryCount].gameObject.SetActive(true);
            history[HistoryCount].sprite = tempSprite;
        }
        else
        {

            for (int i = 0; i < history.Count - 1; i++)
            {
                history[i].sprite = history[i + 1].sprite;
            }


            history[history.Count - 1].sprite = tempSprite;
        }

    }









    #region Card Handling


    // Determines the sprite for a given card suit and value
    private Sprite CardSet(string suit, int value)
    {

        Sprite tempSprite = null;
        switch (suit.ToUpper())
        {
            case "HEARTS":
                tempSprite = HeartSpriteList[value];
                break;
            case "DIAMONDS":
                tempSprite = DiamondSpriteList[value];
                break;
            case "CLUBS":
                tempSprite = ClubSpriteList[value];
                break;
            case "SPADES":
                tempSprite = SpadeSpriteList[value];
                break;
            default:
                Debug.LogError("Invalid Suit: " + suit);
                break;
        }
        return tempSprite;
    }


    #endregion

    #region Coroutines

    internal void AllCardToggle(bool istrue)
    {
        BlackBtn.interactable = istrue;
        RedBtn.interactable = istrue;

        SpadeBtn.interactable = istrue;
        DiamondBtn.interactable = istrue;
        HeartBtn.interactable = istrue;
        JackBtn.interactable = istrue;
    }
    // Main coroutine for handling the gamble process






    // Coroutine for resetting the game after collection
    IEnumerator Collectroutine()
    {
        yield return new WaitForSeconds(2f);
        gambleStart = false;
        yield return new WaitForSeconds(2);
        slotController.updateBalance();
        if (gamble_game) gamble_game.SetActive(false);

        allcards.ForEach((element) =>
        {
            element.Card_Button.image.sprite = cardCover;
            element.Reset();
        });
        DealerCard_Script.Card_Button.image.sprite = cardCover;
        DealerCard_Script.once = false;
        toggleDoubleButton(false);
        if (isAutoSpinOn)
        {


            slotController.AutoSpin();
        }

    }

    #endregion

    #region Gamble Actions

    // Get the correct card sprite based on the player's result
    internal Sprite GetCard()
    {
        if (DealerCard_Script) DealerCard_Script.cardImage = highcard_Sprite;
        return lowcard_Sprite;


    }

    // Flip all the cards when the game ends


    #endregion

    #region Tweening Animations

    // Controls the scaling animation for the double button
    internal void GambleTweeningAnim(bool IsStart)
    {
        if (IsStart)
        {
            Gamble_Tween_Scale = doubleButton.gameObject.GetComponent<RectTransform>()
                .DOScale(new Vector2(1.18f, 1.18f), 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(0);
        }
        else
        {
            Gamble_Tween_Scale.Kill();
            doubleButton.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    #endregion
}

[Serializable]
public class cardStruct
{
    public String suit;
    public int value;
}