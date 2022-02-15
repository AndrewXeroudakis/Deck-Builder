using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : Singleton<UIManager>
{
    #region Variables
    public DeckBuilderUIController deckBuilderUIController;
    public MenuUIController menuUIController;
    public LoadingUIController loadingUIController;
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        // Open Main Menu
        menuUIController.Display();
    }
    #endregion

    #region Methods
    public void PlayOptionSelectedSFX()
    {
        AudioManager.Instance.PlaySound("MenuClick");
    }

    public void PlayBackSelectedSFX()
    {
        AudioManager.Instance.PlaySound("MenuBack");
    }
    #endregion
}

