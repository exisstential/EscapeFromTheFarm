using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollector : MonoBehaviour, IData
{
    [SerializeField] private TMP_Text CoinText = null;
    int CoinCount = 0;
    int totalCoinCount = 0;

    public void LoadData(GameData data)
    {
        this.totalCoinCount = data.coinCount;
    }

    public void SaveData(ref GameData data)
    {
        data.coinCount = this.CoinCount + this.totalCoinCount;
    }

    private void Start()
    {
        CoinText.text = CoinCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coin"))
        {
            Destroy(collision.gameObject);

            CoinCount++;
            CoinText.text = CoinCount.ToString();
        }
    }
}
