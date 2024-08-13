using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;

    [SerializeField]
    private float distanceToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger")) {
            Instantiate(roadSection, new Vector3(0, 0, distanceToSpawn), Quaternion.identity);
        }
    }
}
