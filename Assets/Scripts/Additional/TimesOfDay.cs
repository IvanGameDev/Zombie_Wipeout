using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesOfDay : MonoBehaviour
{
    public Transform[] switchObjects;
    internal int switchIndex = 0;

    // The PlayerPrefs record that is used to keep track of the switched objects across multiple playthroughs
    public string playerPrefsName = "TimesOfDay";

    internal int index;
    void Start()
    {
        // Get the index of the current object from the PlayerPrefs record
        switchIndex = PlayerPrefs.GetInt(playerPrefsName, switchIndex);

        // Go through all the objects in the list, hide them, and show only the needed object
        for (index = 0; index < switchObjects.Length; index++)
        {
            if (index == switchIndex) switchObjects[index].gameObject.SetActive(true);
            else switchObjects[index].gameObject.SetActive(false);
        }

        // Go to the next object index in the list
        if (switchIndex < switchObjects.Length - 1) switchIndex++;
        else switchIndex = 0;

        // Record the number so we can use it to show a different object the next time we play
        PlayerPrefs.SetInt(playerPrefsName, switchIndex);
    }
}
