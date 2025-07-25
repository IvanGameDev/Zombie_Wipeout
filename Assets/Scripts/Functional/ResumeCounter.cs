using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResumeCounter : MonoBehaviour
{
    public ResumeCounter instance;
    public ZDGGameController controller;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; 
    }

    public void ShowFuelAd()
    {
        if(controller.deathCounter % 2 == 0)
        {
            controller.infiniteFuelButton.gameObject.SetActive(true);
        }
        else
        {
            controller.infiniteFuelButton.gameObject.SetActive(false);
        }
    }
}
