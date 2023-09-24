using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCanvas : UIController
{
    [SerializeField] private CarController carController;
    [SerializeField] private Text speedText;

    public void Initialize()
    {
        carController.GetSpeedCar  += (text) => 
        {
            speedText.text = $"Speed - {text} km/h";
        };
    }
}
