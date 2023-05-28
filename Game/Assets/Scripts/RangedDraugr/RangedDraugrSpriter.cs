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
    [SerializeField] float arrowSpeed = 20f;
    private Vector3 facing;
    private bool throwable;
    // Start is called before the first frame update
    void Start()
    {
        master = this.GetComponentInParent<RangedDraugr>();
        throwable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting && throwable) {
            GameObject projectile = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<Arrow>().SetAttack(master.secondaryAttack);
            throwable = false;
            Debug.Log("My pointy self just manifested");
            Rigidbody2D projectileRigid2d = projectile.GetComponent<Rigidbody2D>();
            projectileRigid2d.velocity = facing * arrowSpeed;
        }
    }

    public void ShootStart() {
        isShooting = true;
    }

    public void ShootFinish(){
        isShooting = false;
        throwable = true;
    }

    /*
        these functions change the melee attack reach by 
        calling a different child object that acts as a 
        point where a gizmo circle is drawn, said circle
        acts as the attack range and changing its radius
    */

    public void Right() {
        direction = 0f;
        facing.x = 1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(8);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Left() {
        direction = 180f;
        facing.x = -1;
        facing.y = 0;
        shootPoint = this.gameObject.transform.parent.GetChild(6);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Up() {
        direction = 90f;
        facing.x = 0;
        facing.y = 1;
        shootPoint = this.gameObject.transform.parent.GetChild(5);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
    public void Down() {
        direction = 270f;
        facing.x = 0;
        facing.y = -1;
        shootPoint = this.gameObject.transform.parent.GetChild(7);
        shootPoint.rotation = Quaternion.Euler(0, 0, direction);
    }
}
