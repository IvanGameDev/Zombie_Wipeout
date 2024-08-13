using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandSectionSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject grandSectionSpawner;
    [SerializeField]
    private Vector3 nextSpawnPoint;

    public void SpawnSection(){
        GameObject temp = Instantiate(grandSectionSpawner, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

    private void Start(){
        for (int i = 0; i < 2; i++){
            SpawnSection();
        }
    }
}
