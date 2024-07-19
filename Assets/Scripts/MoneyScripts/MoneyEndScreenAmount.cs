using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyEndScreenAmount : MonoBehaviour
{
    public Text endScreenMoneyTxt;

    private void Update()
    {
        endScreenMoneyTxt.text = "Money Collected: " + MoneyScript.moneyCollected.ToString();
    }
}
