using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Color color;
    [SerializeField]  Image fill;

    public void SetMaxHealth(int health) {
        slider.maxValue = health; // Sets maximum health
        slider.value = health; // Sets current health
        fill.color = color;
    }


    public void SetHealth(int health) {
        slider.value = health;
    }
}
