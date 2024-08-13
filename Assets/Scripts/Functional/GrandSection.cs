using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandSection : MonoBehaviour
{
    GrandSectionSpawner grandSectionSpawner;

    private void Start() {
        grandSectionSpawner = GameObject.FindAnyObjectByType<GrandSectionSpawner>();
    }

    private void OnTriggerExit(Collider other){
        grandSectionSpawner.SpawnSection();
        Destroy(gameObject, 2);
    }
}
