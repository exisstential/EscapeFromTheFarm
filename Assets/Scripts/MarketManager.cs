using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketManager : MonoBehaviour, IData
{
    private int CoinCount = 0;
    public TMP_Text MenuCoinText = null, MarketCoinText = null;

    [SerializeField] private List <MarketCharacterInfo> CharacterMarketList = new List<MarketCharacterInfo>();
    [SerializeField] private List<int> ownedCharacterIndexes = new List<int> {0};
    int equipedCharacterIndex = 0;
    [SerializeField] private TMP_Text highScoreText = null;
    int high_score = 0;

    [System.Serializable]
    public struct MarketCharacterInfo
    {
        public string characterName;
        public Sprite characterSprite;
        public int characterPrice;
        public CharacterHolderScript holderScript;

    }

    DataManager dataManagerScript;
    
    public void LoadData(GameData data)
    {
        this.CoinCount = data.coinCount;
        this.equipedCharacterIndex = data.equipedCharacterIndex;
        this.ownedCharacterIndexes = data.ownedCharactersList;
        this.high_score = data.highScore;

        highScoreText.text = "High Score: " + high_score.ToString();

        MarketCoinText.text = CoinCount.ToString();
        MenuCoinText.text = CoinCount.ToString();

            for(int i = 0; i < CharacterMarketList.Count; i++)
            {
                if (!ownedCharacterIndexes.Contains(i))
                {
                    CharacterMarketList[i].holderScript.NotOwned();
                }
            }

            foreach(int _index in ownedCharacterIndexes)
            {
                if (_index == equipedCharacterIndex)
                {
                    CharacterMarketList[equipedCharacterIndex].holderScript.Owned(true);
                }
                else
                {
                    CharacterMarketList[_index].holderScript.Owned(false);
                }
            }
    }

    public void SaveData(ref GameData data)
    {
        data.coinCount = this.CoinCount;
        data.equipedCharacterIndex = this.equipedCharacterIndex;
        data.ownedCharactersList = this.ownedCharacterIndexes;
    }
    private void Start()
    {
        for(int i = 0; i < CharacterMarketList.Count; i++)
        {
            CharacterMarketList[i].holderScript.SetProperties(CharacterMarketList[i].characterName,
             CharacterMarketList[i].characterSprite, CharacterMarketList[i].characterPrice, this, i);
        }

        dataManagerScript = FindObjectOfType<DataManager>();
    }

    public void BuyOrEquip(int requestIndex)
    {
        if (ownedCharacterIndexes.Contains(requestIndex))
        {
            CharacterMarketList[equipedCharacterIndex].holderScript.Owned(false);//before
            equipedCharacterIndex = requestIndex;//after
            CharacterMarketList[requestIndex].holderScript.Owned(true);
            dataManagerScript.SaveGame();
        }
        else
        {
            if (CoinCount  >= CharacterMarketList[requestIndex].characterPrice)
            {
                CharacterMarketList[equipedCharacterIndex].holderScript.Owned(false);
                ownedCharacterIndexes.Add(requestIndex);
                equipedCharacterIndex = requestIndex;
                CharacterMarketList[requestIndex].holderScript.Owned(true);
                CoinCount -=CharacterMarketList[requestIndex].characterPrice;
                FindObjectOfType<DataManager>().SaveGame();
                MarketCoinText.text = CoinCount.ToString();
                MenuCoinText.text = CoinCount.ToString();
                dataManagerScript.SaveGame();
            }
        }
    }
}
