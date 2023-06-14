/*
    Dialogue.cs
    Make the dialogue of the NPC appear when the player is near
    and show text progressively.

    Joaquin Badillo
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    [SerializeField] float delay;
    [SerializeField] string text;
    [SerializeField] TextMesh dialogueText;

    [SerializeField] GameObject frame;

    int charIndex = 0;

    [SerializeField] bool giveCoins = false;
    [SerializeField] CanvasGroup canvasGroup;

    private Coroutine dialogueCoroutine;
    bool finished;
    // Start is called before the first frame update
    void Start() {
        dialogueText.text = "";
        frame.SetActive(false);
        text = text.Replace("`", PlayerPrefs.GetString("username"));
        text = text.Replace("_", "\n");
    }

    public void SetText(string _text) {
        this.text = _text;
    }

    IEnumerator ShowDialogue() {        
        while (charIndex < text.Length) {
            dialogueText.text += text[charIndex];
            charIndex++;
            yield return new WaitForSeconds(delay);
        }

        if (!giveCoins)
            yield break;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddCoins(200);
        giveCoins = false;
        
        StartCoroutine(ShowAchievement());
        yield break;
    }

    IEnumerator ShowAchievement() {
        for (float f = 0f; f <= 1; f += 0.05f) {
            canvasGroup.alpha = f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.5f);

        for (float f = 0f; f >= 0 ; f -= 0.05f) {
            canvasGroup.alpha = f;
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            frame.SetActive(true);
            dialogueCoroutine = StartCoroutine(ShowDialogue());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            frame.SetActive(false);
            dialogueText.text = "";
            charIndex = 0;
            StopCoroutine(dialogueCoroutine);
        }
    }
}
