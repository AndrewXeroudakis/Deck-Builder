using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DeckUI : MonoBehaviour
{
    #region Variables
    // Components
    Button button;
    TMP_Text title;

    // Fields
    Deck deck;
    public Deck Deck { get { return deck; } private set { deck = value; } }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        GetComponents();
    }
    #endregion

    #region Methods
    void GetComponents()
    {
        title = GetComponentInChildren<TMP_Text>();
        title.text = "Deck #" + deck.id;
        button = GetComponent<Button>();

        button.onClick.AddListener(delegate { UIManager.Instance.deckBuilderUIController.DisplayDeck(this); });
    }

    public void SetDeck(Deck _deck)
    {
        deck = _deck;
        //SetTitle();
    }
    /*void SetTitle()
    {
        StartCoroutine(SetTitleWithDelay());
    }

    IEnumerator SetTitleWithDelay()
    {
        yield return new WaitForSeconds(0.001f);
        title.text = "Deck #" + deck.id;
    }*/
    #endregion
}
