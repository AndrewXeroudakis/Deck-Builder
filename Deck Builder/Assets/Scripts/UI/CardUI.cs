using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonTcgSdk;


public class CardUI : Draggable
{
    #region Variables
    // Components
    RawImage rawImage;

    // Fields
    string id;
    public string ID { get { return id; } private set { id = value; } }
    string imageURL;
    public string ImageURL { get { return imageURL; } private set { imageURL = value; } }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(imageURL))
            SetImage();
    }
    #endregion

    #region Methods
    public void SetID(string _id) => id = _id;
    public void SetImageURL(string _imageUrl) => imageURL = _imageUrl;

    void GetComponents()
    {
        rawImage = GetComponentInChildren<RawImage>();
    }

    void SetImage()
    {
        rawImage.enabled = false;
        WebManager.Instance.RequestImage(imageURL, rawImage);
    }

    public void SetRawImage(Texture _rawImage) => rawImage.texture = _rawImage;
    #endregion
}


