using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{ 
    // Format string according to corresponding animation
    public string GetAnimation(string blessed, string wound, string weapon, string direction, string action) {
        string animation = "female-archer-" + blessed + "-" + wound + "-" + weapon + "-" + direction + "-" + action;
        return animation;
    }
}
