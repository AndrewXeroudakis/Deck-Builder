using UnityEngine;
using UnityEngine.UI;

public class CardUI : Draggable
{
    #region Variables
    // Components
    Button button;
    RawImage rawImage;

    // Fields
    string id;
    public string ID { get { return id; } private set { id = value; } }
    new string name;
    public string Name { get { return name; } private set { name = value; } }
    string type;
    public string Type { get { return type; } private set { type = value; } }
    int hp;
    public int Hp { get { return hp; } private set { hp = value; } }
    string rarity;
    public string Rarity { get { return rarity; } private set { rarity = value; } }
    string imageURL;
    public string ImageURL { get { return imageURL; } private set { imageURL = value; } }
    public bool isInMyCollection;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(imageURL)
            && !string.IsNullOrEmpty(id))
            SetImage(id);
    }
    #endregion

    #region Methods
    public void SetFields(string _id, string _name, string _type, int _hp, string _rarity, string _imageURL, bool _isInMyCollection)
    {
        id = _id;
        name = _name;
        type = _type;
        hp = _hp;
        rarity = _rarity;
        imageURL = _imageURL;
        isInMyCollection = _isInMyCollection;
    }

    void GetComponents()
    {
        rawImage = GetComponentInChildren<RawImage>();
        button = GetComponent<Button>();

        if (isInMyCollection)
            button.onClick.AddListener(delegate { UIManager.Instance.deckBuilderUIController.AddCardToMyDeck(this); });
        else
            button.onClick.AddListener(delegate { UIManager.Instance.deckBuilderUIController.RemoveCardFromMyDeck(this); });
    }

    public void SetImage(string _imgID)
    {
        // Load Texture
        if (SaveLoadManager.LoadTexture2D(_imgID, SaveLoadManager.Ext.JPG, out Texture2D texture))
            rawImage.texture = texture;
        else
        {
            // Get image url
            int index = WebManager.Instance.DataRoot.data.FindIndex(d => d.id.Equals(_imgID));

            if (index > -1)
            {
                // Get image url
                string imgURL = WebManager.Instance.DataRoot.data[index].images.small;

                // Download texture and then set it
                StartCoroutine(WebManager.Instance.GetImage(imgURL, _imgID, SetImage));

                // Hide raw image
                rawImage.enabled = false;
            }
        }
    }

    public void SetImage(Texture2D _texture)
    {
        rawImage.texture = _texture;

        // Display raw image
        rawImage.enabled = true;
    }
    #endregion
}


