using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionMovement : MonoBehaviour
{
    [SerializeField]
    private float sectionSpeed;

    private void Update()
    {
        transform.position += new Vector3(0, 0, sectionSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyer")){
            Destroy(gameObject);
        }
    }
}
