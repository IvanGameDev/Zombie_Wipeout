using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSound : MonoBehaviour
{
    public AudioSource zombieExplode;
    public AudioSource woodenObstacle;
    public AudioSource vehicleExplode;
    public AudioSource barrellExplode;
    public AudioSource pickUp;
    public AudioSource buttonSounds;

    public void TurnSoundOff()
    {
        barrellExplode.enabled = false;
        vehicleExplode.enabled = false;
        woodenObstacle.enabled = false;
        zombieExplode.enabled = false;
        pickUp.enabled = false;
        buttonSounds.enabled = false;
    }

    public void TurnSoundOn()
    {
        barrellExplode.enabled = true;
        vehicleExplode.enabled = true;
        woodenObstacle.enabled = true;
        zombieExplode.enabled = true; 
        pickUp.enabled = true;
        buttonSounds.enabled = true;
    }
}
