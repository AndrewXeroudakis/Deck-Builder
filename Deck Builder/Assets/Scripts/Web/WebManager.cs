using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/*using PokemonTcgSdk;
using PokemonTcgSdk.Models;*/
using UnityEngine.Networking;

public class WebManager : Singleton<WebManager>
{
    #region Variables
    [Header("Base Url to download json from")]
    public string baseURL = "https://api.pokemontcg.io/v2/cards";

    [Header("The name of the folder we want to save the json to")]
    public string dataFolderName = "data";

    [Header("The name of the folder we want to save the images to")]
    public string imagesFolderName = "images";

    [Space]
    [Header("Downloaded data")]
    [SerializeField]
    private Root dataRoot;
    public Root DataRoot { get { return dataRoot; } private set { dataRoot = value; } }

    static private readonly string baseSetDataName = "baseSet";
    static private readonly string APIKey = "40add4bd-38c9-4758-a962-ea9cf3f4831b";

    /*[HideInInspector]
    public bool downloadingImages;*/
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        SaveLoadManager.CreatePathForData(dataFolderName, imagesFolderName);
    }
    #endregion

    #region Methods
    UnityWebRequest SetupRequest(string _url, string _method, object _body)
    {
        // Set request body
        string bodyString = null;
        if (_body is string)
            bodyString = (string)_body;
        else if (_body != null)
            bodyString = JsonUtility.ToJson(_body);

        UnityWebRequest webRequest = new UnityWebRequest();
        webRequest.url = _url;
        webRequest.method = _method;
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
        //webRequest.SetRequestHeader("Accept", "application/json");
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("X-Api-Key", APIKey);

        return webRequest;
    }

    // Gets All Base set card data
    IEnumerator GetJsonData()
    {
        // Display Loading Screen
        UIManager.Instance.loadingUIController.DisplayLoadingScreen(true);

        // Calculate seconds to download
        System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
        stopWatch.Start();

        UnityWebRequest webRequest = SetupRequest(baseURL + "?q=set.id:base1&q=supertype:Pokemon&pageSize=100", UnityWebRequest.kHttpVerbGET, null);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);

            stopWatch.Stop();
            TimeSpan timeSpan = stopWatch.Elapsed;
            Debug.Log("Failed to download data after " + (float)timeSpan.TotalSeconds + " seconds");

            yield return "fail";
        }
        else
        {
            string jsonData = webRequest.downloadHandler.text;
            SaveLoadManager.SaveStringAsJsonFile(jsonData, baseSetDataName, true);

            stopWatch.Stop();
            TimeSpan timeSpan = stopWatch.Elapsed;
            Debug.Log("Downloaded Data in " + (float)timeSpan.TotalSeconds + " seconds");

            // Load data
            LoadDataLocally();

            // Hide Loading Screen
            UIManager.Instance.loadingUIController.DisplayLoadingScreen(false);

            // Instance my card collection and download images
            UIManager.Instance.deckBuilderUIController.InstantiateMyCardCollection();

            yield return "success";
        }
        yield break;
    }

    IEnumerator GetImage(string _imgUrl, string _id)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_imgUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                byte[] bytes = ((DownloadHandlerTexture)uwr.downloadHandler).data;
                SaveLoadManager.SaveImageData(bytes, _id, SaveLoadManager.Ext.JPG); //Path.GetFileNameWithoutExtension(_imgUrl)
            }
        }
    }

    public IEnumerator GetImage(string _imgUrl, string _id, Action<Texture2D> _action)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_imgUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                _action?.Invoke(texture);

                byte[] bytes = ((DownloadHandlerTexture)uwr.downloadHandler).data;
                SaveLoadManager.SaveImageData(bytes, _id, SaveLoadManager.Ext.JPG);
            }
        }
    }

    public void DownloadData()
    {
        // Get json and save it locally
        StartCoroutine(GetJsonData());
    }

    public void LoadDataLocally()
    {
        string jsonData = SaveLoadManager.LoadDataJsonAsString(baseSetDataName);
        dataRoot = JsonUtility.FromJson<Root>(jsonData);
    }

    /*public void DownloadImages()
    {
        if (dataRoot.data == null)
            return; // failed reading json

        // Download images
        foreach (Datum datum in dataRoot.data)
        {
            if (!string.IsNullOrWhiteSpace(datum.images.small))
            {
                StartCoroutine(GetImage(datum.images.small, datum.id));
            }
        }
    }*/

    //public void GetCards()
    /*public void RequestImage(string _url, RawImage _rawImage)
    {
        StartCoroutine(DownloadImage(_url, _rawImage));
    }

    IEnumerator DownloadImage(string _url, RawImage _rawImage)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url);
        webRequest.SetRequestHeader("X-Api-Key", APIKey);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(webRequest.error);
            yield break;
        }
        else
        {
            _rawImage.texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            _rawImage.enabled = true;
        }
    }*/
    #endregion
}


#region Data casting
[Serializable]
public class Root
{
    public List<Datum> data;
    public int page;
    public int pageSize;
    public int count;
    public int totalCount;
}

[Serializable]
public class Datum
{
    public string id;
    public string name;
    public string supertype;
    public List<string> subtypes;
    public string hp;
    public List<string> types;
    public List<string> evolvesTo;
    public List<string> rules;
    public List<Attack> attacks;
    public List<Weakness> weaknesses;
    public List<string> retreatCost;
    public int convertedRetreatCost;
    public Set set;
    public string number;
    public string artist;
    public string rarity;
    public List<int> nationalPokedexNumbers;
    public Legalities legalities;
    public Images images;
    public Tcgplayer tcgplayer;
    public Cardmarket cardmarket;
}

[Serializable]
public class Attack
{
    public string name;
    public List<string> cost;
    public int convertedEnergyCost;
    public string damage;
    public string text;
}

[Serializable]
public class Weakness
{
    public string type;
    public string value;
}

[Serializable]
public class Legalities
{
    public string unlimited;
    public string expanded;
}

[Serializable]
public class Images
{
    public string symbol;
    public string logo;
    public string small;
    public string large;
}

[Serializable]
public class Set
{
    public string id;
    public string name;
    public string series;
    public int printedTotal;
    public int total;
    public Legalities legalities;
    public string ptcgoCode;
    public string releaseDate;
    public string updatedAt;
    public Images images;
}

[Serializable]
public class Holofoil
{
    public double low;
    public double mid;
    public double high;
    public double market;
    public double directLow;
}

[Serializable]
public class Prices
{
    public Holofoil holofoil;
}

[Serializable]
public class Tcgplayer
{
    public string url;
    public string updatedAt;
    public Prices prices;
}

[Serializable]
public class Cardmarket
{
    public string url;
    public string updatedAt;
    public Prices prices;
}

[Serializable]
public class Deck
{
    public string id;
    public List<string> cardIds;
}
#endregion

