using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{
    public int coinCount;
    public int equipedCharacterIndex;
    public List<int> ownedCharactersList = new List<int>();
    public int highScore;

    public GameData()
    {
        this.coinCount = 0;
        this.equipedCharacterIndex = 0;
        this.ownedCharactersList = new List<int> { 0 };
        this.highScore = 0;
    }
}
