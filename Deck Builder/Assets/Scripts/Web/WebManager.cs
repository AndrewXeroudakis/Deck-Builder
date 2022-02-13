using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using PokemonTcgSdk;

using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Net;
using System;

namespace PokemonTcgSdk.Models
{
    public class WebManager : Singleton<WebManager>
    {
        #region Variables

        public TMP_Text testText;

        static private readonly string basePokemonURL = "https://api.pokemontcg.io/v2/";
        static private readonly string APIKey = "40add4bd-38c9-4758-a962-ea9cf3f4831b";
        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            testText.text = "Loading...";
        }

        private void Start()
        {
            StartCoroutine(GetCardByName("charizard"));
        }
        #endregion


        #region Methods
        UnityWebRequest SetupRequest(string _url, string _method, object _body)
        {
            string bodyString = null;

            if (_body is string)
            {
                bodyString = (string)_body;
            }
            else if (_body != null)
            {
                bodyString = JsonUtility.ToJson(_body);
            }

            UnityWebRequest webRequest = new UnityWebRequest();

            webRequest.url = _url;
            webRequest.method = _method;
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            //webRequest.
            webRequest.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : Encoding.UTF8.GetBytes(bodyString));
            //webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("X-Api-Key", APIKey);

            return webRequest;
        }

        IEnumerator GetCardByName(string _name)
        {
            string query = string.Format("!name:{0}", _name);

            Dictionary<string, string> body = new Dictionary<string, string> { { "", "" } };

            string cardURL = string.Format("{0}cards", basePokemonURL);


            //"https://api.pokemontcg.io/v2/cards?" + "q=!name:charizard" + "&pageSize=1"
            //UnityWebRequest webRequest = SetupRequest("https://api.pokemontcg.io/v2/cards?" + "pageSize=50", UnityWebRequest.kHttpVerbGET, body);//cardURL //https://api.pokemontcg.io/v2/cards/pl1-1

            UnityWebRequest webRequest = SetupRequest("https://api.pokemontcg.io/v2/cards?" + "q=set.id:base1" + "&pageSize=102", UnityWebRequest.kHttpVerbGET, body);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) 
            {
                Debug.LogError(webRequest.error);
                yield break;
            }

            // Get byte[] data
            byte[] data = webRequest.downloadHandler.data;

            if (data != null)
            {
                string jsonString = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log(jsonString);

                // Cards
                /*Root root = JsonUtility.FromJson<Root>(jsonString);
                Debug.Log(root.data[48].name);
                testText.text = root.data[48].name.ToString();*/

                // Cards
                Root root = JsonUtility.FromJson<Root>(jsonString);
                Debug.Log(root.data[0].set.series);
                testText.text = root.data[0].set.series;
            }
        }
        #endregion
    }

    /*public class Ability
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
    }*/

    #region Cards
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

    #region Sets
    /*[Serializable]
    public class SetLegalities
    {
        public string unlimited;
        public string standard;
        public string expanded;
    }

    [Serializable]
    public class SetImages
    {
        public string symbol;
        public string logo;
    }

    [Serializable]
    public class SetRoot
    {
        public string id;
        public string name;
        public string series;
        public int printedTotal;
        public int total;
        public SetLegalities legalities;
        public string ptcgoCode;
        public string releaseDate;
        public string updatedAt;
        public SetImages images;
    }*/
    #endregion
}

