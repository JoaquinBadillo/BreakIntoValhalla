/*
    Simple scriptable object to facilitate 
    switching between the local and remote api

    Joaquín Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HostData", menuName = "HostSO", order = 1)]
public class HostSO : ScriptableObject {
    public string uri;
}
