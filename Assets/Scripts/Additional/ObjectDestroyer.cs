using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private GameObject money;

    private void Start()
    {
        money = GameObject.FindGameObjectWithTag("Cash50");
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Cash50")
        {
            Destroy(gameObject);
        }
    }
}
