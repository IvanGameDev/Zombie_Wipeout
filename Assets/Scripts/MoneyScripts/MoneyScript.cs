using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyScript : MonoBehaviour
{
    public static MoneyScript instance;
    public Text moneyCollectedTxt;

    public Transform moneyEffect;
    public AudioSource pickUpMoney;

    public static int moneyCollected;
    public bool moneyExplode;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        moneyExplode = false;
        moneyCollectedTxt = GetComponent<Text>();
        pickUpMoney = GetComponent<AudioSource>();
    }

    private void Update()
    {
        moneyCollectedTxt.text = " " + moneyCollected.ToString();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            moneyCollected += 50;
            moneyExplode = true;
            if (moneyExplode)
            {
                Instantiate(moneyEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            pickUpMoney.Play();
        }
    }
}
