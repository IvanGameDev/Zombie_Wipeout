using UnityEngine;
using System.Collections;

    public class ZDGPlayer : MonoBehaviour
    {
        [Tooltip("The health of the player. If this reaches 0, the player dies")]
        public float health = 100;
        internal float healthMax;

        [Tooltip("The fuel of the player. If this reaches 0, the game ends")]
        public float fuel = 100;
        internal float fuelMax;

        [Tooltip("The speed of the player, how fast it moves player")]
        public float speed = 10;

        [Tooltip("How quickly the player changes direction from left to right and back")]
        public float turnSpeed = 100;

        [Tooltip("The maximum angle to which the player can turn. This is both for right and left directions")]
        public float turnRange = 25;

        [Tooltip("The effect that appears when this player dies")]
        public Transform deathEffect;

        public AudioSource AudioSource;

        public Transform moneyEffect;

        public void Die()
        {
            // Create a death effect at the position of the player
            if (deathEffect) Instantiate(deathEffect, transform.position, transform.rotation);

            // Remove the player from the game
            Destroy(gameObject);
        }
    }
