using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DeckBuilderUIController : MonoBehaviour
{
    #region Variables
    [Header("UI Elements")]
    [SerializeField]
    GameObject deckBuilderScreen;
    [SerializeField]
    GameObject decksContainer;
    [SerializeField]
    GameObject createDeckContainer;
    [Space]
    [Header("Header")]
    [SerializeField]
    Button backButton;
    [SerializeField]
    Button saveDeckButton;
    [SerializeField]
    Button quitButton;
    [Space]
    [Header("Your Decks")]
    [SerializeField]
    Button createDeckButton;
    [SerializeField]
    Transform myDecksContainer;
    [Space]
    [Header("My Deck")]
    [SerializeField]
    Transform myDeckCardsContainer;
    List<DeckUI> myDecks;
    //[SerializeField]
    //TMP_Text cardCounterText;
    List<CardUI> myDeck;
    [Space]
    [Header("My Collection")]
    [SerializeField]
    Transform myCollectionCardsContainer;
    [SerializeField]
    TMP_Dropdown orderByDropdown;
    List<CardUI> myCollection;
    /*[Space]
    [Header("Save Deck")]
    [SerializeField]
    GameObject saveDeckPanel;
    [SerializeField]
    Button saveYesButton;*/
    [Space]
    [Header("Prefabs")]
    [SerializeField]
    GameObject cardUIPrefab;
    [SerializeField]
    GameObject deckUIPrefab;

    string currentDeck;
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        SubscribeButtons();
        InitializeVariables();
    }
    #endregion

    #region Methods
    public void Display()
    {
        // Hide createDeckContainer
        createDeckContainer.SetActive(false);

        // Hide extra Buttons
        backButton.transform.parent.gameObject.SetActive(false);
        saveDeckButton.transform.parent.gameObject.SetActive(false);

        // Load Decks

        // Instantiate Decks

        // Display
        deckBuilderScreen.SetActive(true);
        decksContainer.SetActive(true);
    }

    void SubscribeButtons()
    {
        // Header
        backButton.onClick.AddListener(BackButtonPressed);
        saveDeckButton.onClick.AddListener(SaveDeckButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);

        // Decks
        createDeckButton.onClick.AddListener(CreateDeckButtonPressed);

        // Create Deck
        orderByDropdown.onValueChanged.AddListener(delegate {OrderByDropdownValueChanged();}); 
/*
        // Save Deck
        saveYesButton.onClick.AddListener(SaveYesButtonPressed);*/
    }

    void InitializeVariables()
    {
        myDecks = new List<DeckUI>();
        myDeck = new List<CardUI>();
        myCollection = new List<CardUI>();
        currentDeck = "-1";
    }

    #region Header
    void BackButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Discard cards
        currentDeck = "-1";
        DiscardMyDeck();

        // Display Decks
        Display();
    }

    void SaveDeckButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Save Deck
        SaveDeck();

        // Discard cards
        currentDeck = "-1";
        DiscardMyDeck();

        // Display Decks
        Display();
    }

    void QuitButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Close Deck Builder
        deckBuilderScreen.SetActive(false);

        // Discard cards
        currentDeck = "-1";
        DiscardMyDeck();

        // Display Menu
        UIManager.Instance.menuUIController.Display();
    }
    #endregion

    #region Your Decks
    void CreateDeckButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Hide decks container
        decksContainer.SetActive(false);

        // Display extra Buttons
        backButton.transform.parent.gameObject.SetActive(true);
        saveDeckButton.transform.parent.gameObject.SetActive(true);
        
        // Instantiate Card Collection
        if (myCollection.Count <= 0)
            InstantiateMyCardCollection();

        // Display create deck container
        createDeckContainer.SetActive(true);
    }

    public void DisplayDeck(DeckUI _deck)
    {
        // Set current deck
        currentDeck = _deck.Deck.id;

        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Hide decks container
        decksContainer.SetActive(false);

        // Display extra Buttons
        backButton.transform.parent.gameObject.SetActive(true);
        saveDeckButton.transform.parent.gameObject.SetActive(true);

        // Instantiate Card Collection
        if (myCollection.Count <= 0)
            InstantiateMyCardCollection();

        // Instantiate My Deck
        InstantiateMyDeck(_deck);

        // Display create deck container
        createDeckContainer.SetActive(true);
    }

    public void InstantiateMyDeck(DeckUI _deck)
    {
        myDeck = new List<CardUI>();

        foreach (string cardId in _deck.Deck.cardIds)
        {
            CardUI cardUIInMyCollection = myCollection.Find(c => c.ID.Equals(cardId));

            // Instantiate card
            GameObject newCardUI = Instantiate(cardUIPrefab, myDeckCardsContainer.position, myDeckCardsContainer.rotation, myDeckCardsContainer);
            CardUI cardUI = newCardUI.GetComponent<CardUI>();
            cardUI.SetFields(cardUIInMyCollection.ID, cardUIInMyCollection.Name, cardUIInMyCollection.Type, cardUIInMyCollection.Hp, cardUIInMyCollection.Rarity, cardUIInMyCollection.ImageURL, false);
            myDeck.Add(cardUI);
        }

        RebuildUI();
    }

    public void AddCardToMyDeck(CardUI _cardUI)
    {
        // Play sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Instantiate card
        GameObject newCardUI = Instantiate(cardUIPrefab, myDeckCardsContainer.position, myDeckCardsContainer.rotation, myDeckCardsContainer);
        CardUI cardUI = newCardUI.GetComponent<CardUI>();
        cardUI.SetFields(_cardUI.ID, _cardUI.Name, _cardUI.Type, _cardUI.Hp, _cardUI.Rarity, _cardUI.ImageURL, false);
        myDeck.Add(cardUI);

        RebuildUI();
    }

    public void RemoveCardFromMyDeck(CardUI _cardUI)
    {
        // Play sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Remove card
        myDeck.Remove(_cardUI);

        // Destroy card
        Destroy(_cardUI.gameObject);

        RebuildUI();
    }

    void DiscardMyDeck()
    {
        myDeck.Clear();

        // Destroy cards
        foreach (Transform cardUI in myDeckCardsContainer)
            Destroy(cardUI.gameObject);
    }

    void RebuildUI()
    {
        StartCoroutine(Rebuild());
    }

    IEnumerator Rebuild()
    {
        yield return new WaitForSeconds(0.001f);

        // Dirty Trick
        myDeckCardsContainer.gameObject.SetActive(false);
        myDeckCardsContainer.gameObject.SetActive(true);
    }
    #endregion

    #region My Collection
    public void InstantiateMyCardCollection()
    {
        myCollection = DeckManager.Instance.InstantiateMyCardCollection(cardUIPrefab, myCollectionCardsContainer);
    }

    void OrderByDropdownValueChanged()
    {
        int index = orderByDropdown.value;
        Debug.Log(index);
        if (index == 0)
        {
            myCollection = myCollection.OrderBy(x => x.Name).ToList();
            for (int i = myCollection.Count - 1; i >= 0; i--)
                myCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 1)
        {
            myCollection = myCollection.OrderBy(x => x.Type).ToList();
            for (int i = myCollection.Count - 1; i >= 0; i--)
                myCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 2)
        {
            myCollection = myCollection.OrderBy(x => x.Hp).ToList();
            for (int i = myCollection.Count - 1; i >= 0; i--)
                myCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 3)
        {
            myCollection = myCollection.OrderBy(x => x.Rarity).ToList();
            for (int i = myCollection.Count - 1; i >= 0; i--)
                myCollection[i].transform.SetSiblingIndex(0);
        }
    }
    #endregion

    #region SaveDeck
    /*void SaveYesButtonPressed()
    {

    }*/

    void SaveDeck()
    {
        // Add new Deck
        if (currentDeck.Equals("-1"))
        {
            // Initialize new Deck
            Deck deck = new Deck();
            deck.cardIds = new List<string>();

            // Set deck id
            deck.id = (myDecks.Count + 1).ToString();

            // Set card ids
            foreach (CardUI card in myDeck)
                deck.cardIds.Add(card.ID);

            // Instantiate deckUI
            GameObject newDeckUI = Instantiate(deckUIPrefab, myDecksContainer.position, myDecksContainer.rotation, myDecksContainer);
            DeckUI deckUI = newDeckUI.GetComponent<DeckUI>();
            deckUI.SetDeck(deck);

            // Add deck to decks
            myDecks.Add(deckUI);
        }
        else
        {
            int deckIndex = System.Int32.Parse(currentDeck) - 1;

            // Initialize new Deck
            Deck deck = new Deck();
            deck.cardIds = new List<string>();

            // Set deck id
            deck.id = (deckIndex + 1).ToString();

            // Set card ids
            foreach (CardUI card in myDeck)
                deck.cardIds.Add(card.ID);

            // Set deck
            myDecks[deckIndex].SetDeck(deck);
        }
    }
    #endregion

    #endregion
}



