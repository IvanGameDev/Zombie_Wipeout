using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "ScriptableObjects/Car")]
public class Car : ScriptableObject
{
    [Header("Description")]
    public string carName;
    public string carDescription;

    [Header("Stats")]
    public int carPrice;
    public float durability;
    public float fuel;

    [Header("3D model")]
    public GameObject carModel;
}
