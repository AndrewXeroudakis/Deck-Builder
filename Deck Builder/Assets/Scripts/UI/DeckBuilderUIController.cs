using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckBuilderUIController : MonoBehaviour
{
    #region Variables
    [Header("Header")]
    /*[SerializeField]
    Button saveDeckButton;
    [SerializeField]
    Button backButton;*/
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
    //TMP_Dropdown orderByDropdown;
    List<CardUI> myCollectionCards;
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
    public void Open()
    {

    }

    void SubscribeButtons()
    {
        // Header
        /*saveDeckButton.onClick.AddListener(SaveDeckButtonPressed);
        backButton.onClick.AddListener(BackButtonPressed);

        // My Deck

        // My Collection
        orderByDropdown.onValueChanged.AddListener(delegate {OrderByDropdownValueChanged();}); 

        // Save Deck
        saveYesButton.onClick.AddListener(SaveYesButtonPressed);*/
    }

    void InitializeVariables()
    {
        myDeckCards = new List<CardUI>();
        myCollectionCards = new List<CardUI>();
    }

    #region Header
    void SaveDeckButtonPressed()
    {

    }

    void BackButtonPressed()
    {

    }
    #endregion

    #region My Collection
    public void InstantiateMyCardCollection()
    {
        myCollectionCards = DeckManager.Instance.InstantiateMyCardCollection(cardUIPrefab, myCollectionCardsContainer);
    }

    void OrderByDropdownValueChanged()
    {
        // Switch dropdown values
        // Deck Builder OrderBy
        //Debug.Log(orderByDropdown.value);
    }
    #endregion

    #region SaveDeck
    void SaveYesButtonPressed()
    {

    }
    #endregion

    #endregion
}
