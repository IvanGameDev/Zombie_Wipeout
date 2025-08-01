using UnityEngine;
using System.Collections;
using ZombieDriveGame;

public class ZDGPlayer : MonoBehaviour
    {
        public RewardedAdmob admobReward;
        
        [Tooltip("The health of the player. If this reaches 0, the player dies")]
        public float health = 100;
        public float healthLeft;
        internal float healthMax;

        [Tooltip("The fuel of the player. If this reaches 0, the game ends")]
        public float fuel = 100;
        public float fuelLeft;
        internal float fuelMax;

        [Tooltip("The speed of the player, how fast it moves player")]
        public float speed = 10;

        [Tooltip("How quickly the player changes direction from left to right and back")]
        public float turnSpeed = 100;

        [Tooltip("The maximum angle to which the player can turn. This is both for right and left directions")]
        public float turnRange = 25;

        [Tooltip("The effect that appears when this player dies")]
        public Transform deathEffect;

        public AudioSource zombieExplodeAudioSource;
        public AudioSource barrelAudioSource;
        public AudioSource woodenAudioSource;
        public AudioSource pickUpAudioSource;
        public AudioSource vehicleExplodeAudioSource;

        public Transform moneyEffect;
        public GameObject playerCar;
        public Light[] headLights;

        public void Die()
        {
            // Create a death effect at the position of the player
            // Use this line for editing death effect
            if (deathEffect) Instantiate(deathEffect, transform.position, transform.rotation);

            if(deathEffect) vehicleExplodeAudioSource.Play();
            playerCar.SetActive(false);

            DisableLights();

            // Remove the player from the game
            //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie1"))
        {
            zombieExplodeAudioSource.Play();
        }

        if (other.gameObject.CompareTag("Zombie2"))
        {
            zombieExplodeAudioSource.Play();
        }

        if (other.gameObject.CompareTag("Barrel"))
        {
            barrelAudioSource.Play();
        }

        if (other.gameObject.CompareTag("WoodenObstacle"))
        {
            woodenAudioSource.Play();   
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            pickUpAudioSource.Play();
        }

        if (other.gameObject.CompareTag("PoliceZombie"))
        {
            zombieExplodeAudioSource.Play();
        }
    }

    private void DisableLights()
    {
        for (int i = 0; i < headLights.Length; i++)
        {
            headLights[i].enabled = false;
        }
    }

    public void EnableLights()
    {
        for (int i = 0; i < headLights.Length; i++)
        {
            headLights[i].enabled = true;
        }
    }
}
