using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGenerator : MonoBehaviour
{
    public GameObject moneyPrefab;
    public float repeatTime;

    private void Start()
    {
        InvokeRepeating("SpawnMoney", 3f, repeatTime);
    }

    private void SpawnMoney()
    {
        Instantiate(moneyPrefab, transform.position, Quaternion.identity);
    }
}
