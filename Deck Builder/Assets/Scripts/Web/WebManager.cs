using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using PokemonTcgSdk;
using PokemonTcgSdk.Models;*/
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebManager : Singleton<WebManager>
{
    #region Variables
    static private readonly string APIKey = "40add4bd-38c9-4758-a962-ea9cf3f4831b";
    #endregion

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();
    }
    #endregion

    #region Methods
    //public void GetCards()
    public void RequestImage(string _url, RawImage _rawImage)
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
    }
    #endregion
}

#region Cards
public class Ability
{
    public string name { get; set; }
    public string text { get; set; }
    public string type { get; set; }
}

public class Attack
{
    public string name { get; set; }
    public List<string> cost { get; set; }
    public int convertedEnergyCost { get; set; }
    public string damage { get; set; }
    public string text { get; set; }
}

public class Weakness
{
    public string type { get; set; }
    public string value { get; set; }
}

[Serializable]
public class Resistance
{
    public string type;
    public string value;
}

[Serializable]
public class Legalities
{
    public string unlimited;
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
    public int mid;
    public double high;
    public double market;
}

[Serializable]
public class ReverseHolofoil
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
    public ReverseHolofoil reverseHolofoil;
    public double averageSellPrice;
    public double lowPrice;
    public double trendPrice;
    public double reverseHoloSell;
    public double reverseHoloLow;
    public double reverseHoloTrend;
    public double lowPriceExPlus;
    public double avg1;
    public double avg7;
    public double avg30;
    public double reverseHoloAvg1;
    public double reverseHoloAvg7;
    public double reverseHoloAvg30;
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
public class Datum
{
    public string id;
    public string name;
    public string supertype;
    public List<string> subtypes;
    public string level;
    public string hp;
    public List<string> types;
    public string evolvesFrom;
    public List<Ability> abilities;
    public List<Attack> attacks;
    public List<Weakness> weaknesses;
    public List<Resistance> resistances;
    public List<string> retreatCost;
    public int convertedRetreatCost;
    public Set set;
    public string number;
    public string artist;
    public string rarity;
    public string flavorText;
    public List<int> nationalPokedexNumbers;
    public Legalities legalities;
    public Images images;
    public Tcgplayer tcgplayer;
    public Cardmarket cardmarket;
}

[Serializable]
public class Root
{
    public List<Datum> data;
    public int page;
    public int pageSize;
    public int count;
    public int totalCount;
}
#endregion