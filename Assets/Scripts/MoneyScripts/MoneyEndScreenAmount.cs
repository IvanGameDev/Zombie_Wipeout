using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyEndScreenAmount : MonoBehaviour
{
    public TextMeshProUGUI endScreenMoneyTxt;

    private void Update()
    {
        endScreenMoneyTxt.text = " " + MoneyScript.moneyCollected.ToString();
    }
}
