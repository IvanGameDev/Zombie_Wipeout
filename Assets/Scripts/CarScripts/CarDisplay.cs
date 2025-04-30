using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarDisplay : MonoBehaviour
{
    [Header("Description")]
    [SerializeField]
    private TextMeshProUGUI carName;
    [SerializeField]
    private TextMeshProUGUI carDescription;
    [SerializeField]
    private TextMeshProUGUI carPrice;

    [Header("Display")]
    [SerializeField]
    private Image carDurability;
    [SerializeField]
    private Image carFuel;

    [Header("Car Model")]
    [SerializeField]
    private Transform carHolder;


    public void DisplayCar(Car car)
    {
        carName.text = car.carName;
        carDescription.text = car.carDescription;
        carPrice.text = car.carPrice + "$";

        carDurability.fillAmount = car.durability / 100;
        carFuel.fillAmount = car.fuel / 100;

        if(carHolder.childCount > 0)
        {
            Destroy(carHolder.GetChild(0).gameObject);
        }
        Instantiate(car.carModel, carHolder.position, carHolder.rotation, carHolder);
    }
}
