using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneySaver : MonoBehaviour
{
    public TextMeshProUGUI endingMoneySaver;
    public Transform gameOverCanvas;

    public static int moneyValue;
    public bool collecting;

    private void Start()
    {
        endingMoneySaver = GetComponent<TextMeshProUGUI>();
        collecting = true;

        if(PlayerPrefs.HasKey("MoneyCollected"))
        {
            moneyValue = PlayerPrefs.GetInt("MoneyCollected");
        }
    }

    private void Update()
    {
        if(collecting == true)
        {
            //moneyValue += MoneyScript.moneyCollected;
            if(ZDGGameController.instance.isGameOver == true)
            {
                if (MoneyScript.moneyCollected > moneyValue)
                {
                    moneyValue = MoneyScript.moneyCollected;
                }
                collecting = false;
                PlayerPrefs.SetInt("MoneyCollected", moneyValue);
            }
        }

        gameOverCanvas.Find("Base/TotalMoneyPanel/TotalMoney").GetComponent<TextMeshProUGUI>().text = " " + moneyValue.ToString();
    }
}
