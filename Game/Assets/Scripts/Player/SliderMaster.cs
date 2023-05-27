using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Manage UI elements
public class SliderMaster : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField]  Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health; // Sets maximum health
        slider.value = health; // Sets current health
        fill.color = gradient.Evaluate(1f);
    }


    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
