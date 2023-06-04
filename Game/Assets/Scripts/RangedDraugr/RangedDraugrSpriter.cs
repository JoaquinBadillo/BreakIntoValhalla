using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDraugrSpriter : MonoBehaviour
{
    private RangedDraugr master;
    public bool isShooting;
    public Transform shootPoint;
    public GameObject arrow;
    public float direction;
    [SerializeField] float arrowSpeed = 15f;
    private Vector3 facing;
    private bool throwable;
    public bool death;
    private Collider2D myCollider;
    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<RangedDraugr>();
        throwable = true;
        death = false;
        myCollider = this.GetComponentInParent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isShooting && throwable) {
            master.angle = Mathf.Atan2(master.facingDirection.y, master.facingDirection.x) * Mathf.Rad2Deg;
            GameObject projectile = Instantiate(arrow, shootPoint.position, Quaternion.Euler(0, 0, master.angle));
            projectile.GetComponent<DraugrArrow>().SetAttack(master.secondaryAttack);
            projectile.GetComponent<DraugrArrow>().EnemyCollider = myCollider;
            throwable = false;
            Debug.Log("My pointy self just manifested");
            Rigidbody2D projectileRigid2d = projectile.GetComponent<Rigidbody2D>();
            projectileRigid2d.velocity = facing * arrowSpeed;
            projectile.GetComponent<DraugrArrow>().direction = facing;
        }
    }

    public void ShootStart() {
        isShooting = true;
    }

    public void ShootFinish(){
        isShooting = false;
        throwable = true;
    }

    public void endOfAnimation() {
        death = true;
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right() {
        facing.x = master.facingDirection.x;
        facing.y = master.facingDirection.y;
        shootPoint = transform.parent.GetChild(4);
    }
    public void Left() {
        facing.x = master.facingDirection.x;
        facing.y = master.facingDirection.y;
        shootPoint = transform.parent.GetChild(2);
    }
    public void Up() {
        facing.x = master.facingDirection.x;
        facing.y = master.facingDirection.y;
        shootPoint = transform.parent.GetChild(1);
    }
    public void Down() {
        facing.x = master.facingDirection.x;
        facing.y = master.facingDirection.y;
        shootPoint = transform.parent.GetChild(3);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
            master.Attack();
    }
}
