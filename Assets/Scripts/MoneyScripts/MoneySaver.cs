using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySaver : MonoBehaviour
{
    public Text endingMoneySaver;

    public static int moneyValue;
    public bool collecting;

    private void Start()
    {
        endingMoneySaver = GetComponent<Text>();

        if(PlayerPrefs.HasKey("MoneyCollected"))
        {
            moneyValue = PlayerPrefs.GetInt("MoneyCollected");
        }
    }

    private void Update()
    {
        if(collecting == true)
        {
            moneyValue += MoneyScript.moneyCollected;
            if(ZDGGameController.instance.isGameOver == true)
            {
                collecting = false;
                PlayerPrefs.SetInt("MoneyCollected", moneyValue);
            }
        }
        endingMoneySaver.text = "Total Money: " + moneyValue;
    }
}
