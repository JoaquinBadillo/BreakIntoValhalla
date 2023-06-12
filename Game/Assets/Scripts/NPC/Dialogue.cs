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

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            frame.SetActive(true);
            StartCoroutine(ShowDialogue());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            frame.SetActive(false);
            dialogueText.text = "";
            charIndex = 0;
            StopAllCoroutines();
        }
    }
}
