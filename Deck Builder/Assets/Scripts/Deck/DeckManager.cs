/*using PokemonTcgSdk;
using PokemonTcgSdk.Models;*/
using System;
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
    }

    // Instantiate My Collection
    public List<CardUI> InstantiateMyCardCollection(GameObject _prefab, Transform _parent)
    {
        List<CardUI> cardList = new List<CardUI>();

        foreach (Datum pokemonCard in WebManager.Instance.DataRoot.data)
        {
            GameObject newCardUI = Instantiate(_prefab, _parent.position, _parent.rotation, _parent);
            CardUI cardUI = newCardUI.GetComponent<CardUI>();
            cardUI.SetFields(pokemonCard.id, pokemonCard.name, pokemonCard.types[0], Int32.Parse(pokemonCard.hp), pokemonCard.rarity, pokemonCard.images.small, true);
            cardList.Add(cardUI);
        }

        return cardList;
    }
    #endregion
}
