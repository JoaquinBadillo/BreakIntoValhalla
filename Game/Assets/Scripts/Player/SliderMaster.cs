using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Manage UI elements
public class SliderMaster : MonoBehaviour {
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField]  Image fill;

    public void SetMaxValue(int value) {
        slider.maxValue = value; // Sets maximum health
        slider.value = value; // Sets current health
        fill.color = gradient.Evaluate(1f);
    }


    public void SetValue(int value) {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
