using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHolderScript : MonoBehaviour
{
    [SerializeField] private Color NotequipedColor = new();
    [SerializeField] private Color EquipedColor = new();

    [SerializeField] private GameObject BuyHolder = null, EquipHolder = null, BuyOrEquipButton = null;
    [SerializeField] private Image CharacterHolderImage = null;

    [SerializeField] private Image CharacterImage = null;
    [SerializeField] private TMP_Text CharacterNameTxt = null;
    [SerializeField] private TMP_Text CharacterPriceTxt = null;

    MarketManager marketManagerScript;
    int HolderIndex;

    public void Owned(bool equiped)
    {
        if (equiped)
        {
            CharacterHolderImage.color = EquipedColor;
            EquipHolder.SetActive(true);
            BuyHolder.SetActive(false);
            BuyOrEquipButton.SetActive(false);
        }
        else
        {
            CharacterHolderImage.color = NotequipedColor; 
            EquipHolder.SetActive(true);
            BuyHolder.SetActive(false);
            BuyOrEquipButton.SetActive(true);
        }
    }

    public void NotOwned()
    {
        CharacterHolderImage.color = NotequipedColor;
        BuyHolder.SetActive(true);
        EquipHolder.SetActive(false);
        BuyOrEquipButton.SetActive(true);
    }

    public void SetProperties(string characterName, Sprite characterSprite, int characterPrice, MarketManager mManager, int hIndex) 
    {
        CharacterNameTxt.text = characterName;
        CharacterImage.sprite = characterSprite;
        CharacterPriceTxt.text = characterPrice.ToString();
        marketManagerScript = mManager;
        HolderIndex = hIndex;
    }

    public void BuyOrEquip()
    {
        marketManagerScript.BuyOrEquip(HolderIndex);
    }


}
