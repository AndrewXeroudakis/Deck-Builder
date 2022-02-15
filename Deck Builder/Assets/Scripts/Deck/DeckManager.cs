using PokemonTcgSdk;
using PokemonTcgSdk.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    #region Variables
    Pokemon cards;
    public Pokemon Cards { get { return cards; } private set { cards = value; } }
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        //GetCards();
    }
    private void Start()
    {
        //UIManager.Instance.deckBuilderUIController.InstantiateMyCardCollection();
    }
    #endregion

    #region Methods
    // Get Cards from Web Manager
    void GetCards()
    {
        // Get all pokemon cards of the base set
        Dictionary<string, string> query = new Dictionary<string, string>()
        {
            { "name", "Charizard" },
            { "set", "Base" }
        };
        cards = Card.Get(query);
        Debug.Log(cards.Cards.Count);
    }

    // Instantiate My Collection
    public List<CardUI> InstantiateMyCardCollection(GameObject _prefab, Transform _parent)
    {
        List<CardUI> cardList = new List<CardUI>();

        foreach (PokemonCard pokemonCard in Cards.Cards)
        {
            GameObject newCardUI = Instantiate(_prefab, _parent.position, _parent.rotation, _parent);
            CardUI cardUI = newCardUI.GetComponent<CardUI>();
            cardUI.SetID(pokemonCard.Id);
            cardUI.SetImageURL(pokemonCard.ImageUrl);
            cardList.Add(cardUI);
        }

        return cardList;
    }

    // Order By Type
    // Order By HP
    // Order By Rarity
    // Save Player Decks
    // Load Player Decks
    #endregion
}
