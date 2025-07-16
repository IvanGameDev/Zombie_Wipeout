using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResumeCounter : MonoBehaviour
{
    public ResumeCounter instance;

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
}
