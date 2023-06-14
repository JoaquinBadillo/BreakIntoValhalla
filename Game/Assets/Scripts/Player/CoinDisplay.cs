using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinText;

    void Start() {
        coinText.text = "0";
    }

    // Update is called once per frame
    public void UpdateCoins(int coins) {
        coinText.text = coins.ToString();
    }
}
