using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsChange : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject[] scriptableObjects;
    [SerializeField]
    private CarDisplay carDisplay;
    [SerializeField]
    private MapDisplay mapDisplay;

    private int currentIndex;

    private void Awake()
    {
        ChangeScriptableObject(0);
    }

    public void ChangeScriptableObject(int change)
    {
        currentIndex += change;

        if (currentIndex < 0)
        {
            currentIndex = scriptableObjects.Length - 1;
        }else if (currentIndex > scriptableObjects.Length - 1)
        {
            currentIndex = 0; 
        }

        if (mapDisplay != null)
        {
            mapDisplay.DisplayMap((Map)scriptableObjects[currentIndex]);
        }

        if (carDisplay != null)
        {
            carDisplay.DisplayCar((Car)scriptableObjects[currentIndex]);
        }
    }
}
