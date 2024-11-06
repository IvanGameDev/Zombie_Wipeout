using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffMusic : MonoBehaviour
{
    public AudioSource AudioSource;

    public void TurnMusicOff()
    {
        AudioSource.Stop();
    }

    public void TurnOnMusic()
    {
        AudioSource.Play();
    }
}
