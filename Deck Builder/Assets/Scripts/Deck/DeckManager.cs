/*using PokemonTcgSdk;
using PokemonTcgSdk.Models;*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    #region Variables
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        GetCards();
    }
    #endregion

    #region Methods
    // Get Cards from Web Manager
    void GetCards()
    {
        // Load data
        WebManager.Instance.LoadDataLocally();

        // Check if data
        if (WebManager.Instance.DataRoot == null)
            WebManager.Instance.DownloadData();

        // Get all pokemon cards of the base set

            /*Dictionary<string, string> query = new Dictionary<string, string>()
            {
                { "name", "Charizard" },
                { "set", "Base" }
            };
            cards = Card.Get(query);
            Debug.Log(cards.Cards.Count);*/
    }

    // Instantiate My Collection
    public List<CardUI> InstantiateMyCardCollection(GameObject _prefab, Transform _parent)
    {
        List<CardUI> cardList = new List<CardUI>();

        foreach (Datum pokemonCard in WebManager.Instance.DataRoot.data)
        {
            GameObject newCardUI = Instantiate(_prefab, _parent.position, _parent.rotation, _parent);
            CardUI cardUI = newCardUI.GetComponent<CardUI>();
            cardUI.SetFields(pokemonCard.id, pokemonCard.name, pokemonCard.types[0], pokemonCard.hp, pokemonCard.rarity, pokemonCard.images.small);
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
