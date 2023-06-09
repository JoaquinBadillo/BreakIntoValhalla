using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour {
    protected Player master;
    // Sprite Renderer References
    [SerializeField] protected SpriteRenderer spriter1;
    [SerializeField] protected SpriteRenderer spriter2;
    [SerializeField] protected SpriteRenderer spriter3;

    [SerializeField] protected SpriteRenderer morphingSpriter1;
    [SerializeField] protected SpriteRenderer morphingSpriter2;
    [SerializeField] protected SpriteRenderer morphingSpriter3;

    // Opacity Variables
    [SerializeField] protected float afterimageOpacity1;
    [SerializeField] protected float afterimageOpacity2;
    [SerializeField] protected float afterimageOpacity3;
    [SerializeField] protected float opacityStep;
    [SerializeField] protected float timeStep;
    [SerializeField] protected float timeUntilNextFade;
    // Color Variables
    [SerializeField] protected Color afterimageColor1;
    [SerializeField] protected Color afterimageColor2;
    [SerializeField] protected Color afterimageColor3;
    
    
    // Start is called before the first frame update
    protected void Start() {
        afterimageOpacity1 = 0.38671875f; // 99 opacity
        afterimageOpacity2 = 0.2578125f; // 66 opacity
        afterimageOpacity3 = 0.12890625f; // 33 opacity
        opacityStep = 0.00644531f; // 1.65 opacity per step
        timeStep = 0.01666667f; // 1/60th of a second
        master = GetComponentInParent<Player>();
    }

    protected void Update() {
        if (master.lastmovementDir.x > 0 && master.lastmovementDir.y == 0)
            Right();

        else if (master.lastmovementDir.x < 0 && master.lastmovementDir.y == 0)
            Left();

        else if (master.lastmovementDir.y > 0 && master.lastmovementDir.x == 0)
            Up();

        else if (master.lastmovementDir.y < 0 && master.lastmovementDir.x == 0)
            Down();

        else if (master.lastmovementDir.x > 0 && master.lastmovementDir.y > 0)
            RightUp();

        else if (master.lastmovementDir.x > 0 && master.lastmovementDir.y < 0)
            RightDown();

        else if (master.lastmovementDir.x < 0 && master.lastmovementDir.y > 0)
            LeftUp();

        else if (master.lastmovementDir.x < 0 && master.lastmovementDir.y < 0)
            LeftDown();
    }

    virtual public void Right() {
        spriter1 = master.gameObject.transform.GetChild(19).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(20).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(21).GetComponent<SpriteRenderer>();
    }
    virtual public void Left() {
        spriter1 = master.gameObject.transform.GetChild(13).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(14).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(15).GetComponent<SpriteRenderer>();
    }
    virtual public void Up() {
        spriter1 = master.gameObject.transform.GetChild(10).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(11).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(12).GetComponent<SpriteRenderer>();
    }
    virtual public void Down() {
        spriter1 = master.gameObject.transform.GetChild(16).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(17).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(18).GetComponent<SpriteRenderer>();
    }
    virtual public void LeftUp(){
        spriter1 = master.gameObject.transform.GetChild(22).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(23).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(24).GetComponent<SpriteRenderer>();
    }
    virtual public void RightUp(){
        spriter1 = master.gameObject.transform.GetChild(25).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(26).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(27).GetComponent<SpriteRenderer>();
    }
    virtual public void LeftDown(){
        spriter1 = master.gameObject.transform.GetChild(28).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(29).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(30).GetComponent<SpriteRenderer>();
    }
    virtual public void RightDown(){
        spriter1 = master.gameObject.transform.GetChild(31).GetComponent<SpriteRenderer>();
        spriter2 = master.gameObject.transform.GetChild(32).GetComponent<SpriteRenderer>();
        spriter3 = master.gameObject.transform.GetChild(33).GetComponent<SpriteRenderer>();
    }

    public void ActivateAfterimage() {
        morphingSpriter1 = spriter1;
        morphingSpriter2 = spriter2;
        morphingSpriter3 = spriter3;
        morphingSpriter1.enabled = true;
        morphingSpriter2.enabled = true;
        morphingSpriter3.enabled = true;
        afterimageOpacity1 = 0.38671875f; // 99 opacity
        afterimageOpacity2 = 0.2578125f; // 66 opacity
        afterimageOpacity3 = 0.12890625f; // 33 opacity
        StartCoroutine(DeactivateAfterimage());
    }

    public IEnumerator DeactivateAfterimage() {
        for (int i = 0; i < 60; i++) {
            if (morphingSpriter1 != spriter1) {
                morphingSpriter1.enabled = false;
                morphingSpriter2.enabled = false;
                morphingSpriter3.enabled = false;
                yield break;
            }

            afterimageOpacity3 -= opacityStep;
            afterimageOpacity2 -= opacityStep;
            afterimageOpacity1 -= opacityStep;
    
            morphingSpriter3.color = new Color(1, 1, 1, afterimageOpacity3);
            morphingSpriter2.color = new Color(1, 1, 1, afterimageOpacity2);
            morphingSpriter1.color = new Color(1, 1, 1, afterimageOpacity1);
            yield return new WaitForSeconds(0.01666667f);
        }
        morphingSpriter1.enabled = false;
        morphingSpriter2.enabled = false;
        morphingSpriter3.enabled = false;
        yield break;
    }
}
