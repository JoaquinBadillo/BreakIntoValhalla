using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginScreen : MonoBehaviour {
    EventSystem system;
    public Selectable first;
    public Button submit;
    // Start is called before the first frame update
    void Start() {
        system = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
                next.Select();
            else
                first.Select();
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
            submit.onClick.Invoke();
            Debug.Log("Gotta love the return key");
        }
    }

    // Invalid Credentials, please
    // try again or register
}
