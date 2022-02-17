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
    Button saveDeckButton;
    [SerializeField]
    Button backButton;
    [Space]
    [Header("Your Decks")]
    [SerializeField]
    Button createDeckButton;
    [Space]
    [Header("My Deck")]
    /*[SerializeField]
    Transform myDeckCardsContainer;
    [SerializeField]
    TMP_Text cardCounterText;*/
    List<CardUI> myDeckCards;
    [Space]
    [Header("My Collection")]
    [SerializeField]
    Transform myCollectionCardsContainer;
    [SerializeField]
    TMP_Dropdown orderByDropdown;
    List<CardUI> myCardCollection;
    [Space]
    [Header("Save Deck")]
    /*[SerializeField]
    GameObject saveDeckPanel;
    [SerializeField]
    Button saveYesButton;*/
    [Space]
    [Header("Prefabs")]
    [SerializeField]
    GameObject cardUIPrefab;
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
        deckBuilderScreen.SetActive(true);
        decksContainer.SetActive(true);
    }

    void SubscribeButtons()
    {
        // Header
        //saveDeckButton.onClick.AddListener(SaveDeckButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);

        // Your Decks
        createDeckButton.onClick.AddListener(CreateDeckButtonPressed);

        

        // My Deck

        // My Collection
        orderByDropdown.onValueChanged.AddListener(delegate {OrderByDropdownValueChanged();}); 
/*
        // Save Deck
        saveYesButton.onClick.AddListener(SaveYesButtonPressed);*/
    }

    void InitializeVariables()
    {
        //myDecks = new List<CardUI>();
        myDeckCards = new List<CardUI>();
        myCardCollection = new List<CardUI>();
    }

    #region Header
    void BackButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayBackSelectedSFX();

        // Close Deck Builder
        deckBuilderScreen.SetActive(false);

        // Display Menu
        UIManager.Instance.menuUIController.Display();
    }

    void SaveDeckButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();
    }
    #endregion

    #region Your Decks
    void CreateDeckButtonPressed()
    {
        // Play Sound
        UIManager.Instance.PlayOptionSelectedSFX();

        // Hide decks container
        decksContainer.SetActive(false);

        // Instantiate Card Collection
        if (myCardCollection.Count <= 0)
            InstantiateMyCardCollection();

        // Display create deck container
        createDeckContainer.SetActive(true);
    }
    #endregion

    #region My Collection
    public void InstantiateMyCardCollection()
    {
        myCardCollection = DeckManager.Instance.InstantiateMyCardCollection(cardUIPrefab, myCollectionCardsContainer);
    }

    void OrderByDropdownValueChanged()
    {
        int index = orderByDropdown.value;
        Debug.Log(index);
        if (index == 0)
        {
            myCardCollection = myCardCollection.OrderBy(x => x.Name).ToList();
            for (var i = myCardCollection.Count - 1; i >= 0; i--)
                myCardCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 1)
        {
            myCardCollection = myCardCollection.OrderBy(x => x.Type).ToList();
            for (var i = myCardCollection.Count - 1; i >= 0; i--)
                myCardCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 2)
        {
            myCardCollection = myCardCollection.OrderBy(x => x.Hp).ToList();
            for (var i = myCardCollection.Count - 1; i >= 0; i--)
                myCardCollection[i].transform.SetSiblingIndex(0);
        }
        else if (index == 3)
        {
            myCardCollection = myCardCollection.OrderBy(x => x.Rarity).ToList();
            for (var i = myCardCollection.Count - 1; i >= 0; i--)
                myCardCollection[i].transform.SetSiblingIndex(0);
        }
    }
    #endregion

    #region SaveDeck
    void SaveYesButtonPressed()
    {

    }
    #endregion

    #endregion
}
