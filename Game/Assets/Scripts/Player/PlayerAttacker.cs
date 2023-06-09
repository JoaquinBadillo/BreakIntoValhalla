using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttacker : MonoBehaviour {
    protected Player master;
    private BackgroundMusic musicMaster;
    // Physics Variables
    protected Vector3 facing;
    protected float direction;
    // Stamina variables
    protected WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    protected Coroutine regen;
    [SerializeField] protected GameObject panel;
    // Arrow Reference
    public GameObject arrow;
    [SerializeField] protected float arrowSpeed = 20f;
    // Deflect variables
    private Vector2 deflectDirection;

    // Death Data
    public string currentRoom;

    public string damageSource;
    public string killer;

    // Metrics data
    public int kills = 0;



    virtual public void Bless() {}
    virtual public void DeBless() {}

    protected void RightInitializer(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(4);
        direction = 0f;
        facing.x = 1;
        facing.y = 0;
        master.shootPoint = this.gameObject.transform.parent.GetChild(8);
        master.shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    protected void LeftInitializer(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(2);
        direction = 180f;
        facing.x = -1;
        facing.y = 0;
        master.shootPoint = this.gameObject.transform.parent.GetChild(6);
        master.shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    protected void UpInitializer(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(1);
        direction = 90f;
        facing.x = 0;
        facing.y = 1;
        master.shootPoint = this.gameObject.transform.parent.GetChild(5);
        master.shootPoint.rotation = Quaternion.Euler(0, 0, direction);
        }
    protected void DownInitializer(){
        master.meleeAttackPoint = this.gameObject.transform.parent.GetChild(3);
        direction = 270f;
        facing.x = 0;
        facing.y = -1;
        master.shootPoint = this.gameObject.transform.parent.GetChild(7);
        master.shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }

    protected void Deflect(Collider2D draugrArrow){
        deflectDirection = -draugrArrow.GetComponent<DraugrArrow>().direction;
        draugrArrow.GetComponent<DraugrArrow>().YeetArrow();
        GameObject projectile = Instantiate(arrow, master.shootPoint.position, master.shootPoint.rotation);
        projectile.GetComponent<Arrow>().SetAttack(15);
        projectile.GetComponent<Rigidbody2D>().velocity = deflectDirection * arrowSpeed;
    }

    public void StartAttackSound(){
        master.audio.clip = master.slash;
        master.audio.time = 2.7f;
        master.audio.Play();
    }

    public void UseStamina(int staminaCost) {
        if(master.currentStamina - staminaCost >= 0) {
            master.currentStamina -= staminaCost;
            master.staminaBar.SetValue(master.currentStamina);
            if(regen != null)
                StopCoroutine(regen);
            
            regen = StartCoroutine(RegenStamina());
        }
    }
    
    private IEnumerator RegenStamina() {
        yield return new WaitForSeconds(2f);
        while(master.currentStamina < master.maxStamina) {
            master.currentStamina += master.maxStamina / 100; 
            master.staminaBar.SetValue(master.currentStamina);
            yield return regenTick;
        }
        regen = null; // reset regen
    }
    public void Die() {
        Debug.Log("Died in " + currentRoom + " by " + killer + " with " + kills + " kills");
        PlayerPrefs.SetString("room", currentRoom);
        PlayerPrefs.SetString("killer", killer);
        PlayerPrefs.SetInt("kills", kills);

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine() {
        for (float f = 1f; f >= 0; f -= 0.05f) {
            musicMaster = GameObject.FindGameObjectWithTag("Sounder").GetComponent<BackgroundMusic>();
            musicMaster.audio.volume = f;
            yield return new WaitForSeconds(0.05f);
        }
        musicMaster.audio.Stop();

        for (float f = 0f; f <= 1; f += 0.05f) {
            Color c = new Color(0.074f, 0, 0, f);
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.05f);
        }

        SceneManager.LoadScene("DeathScene");
        yield break;
    }
}
