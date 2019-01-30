using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float last;
    private Animator anim;
    private static float xSpeed;

    public KeyCode shoot = KeyCode.Space;
    public float speed;
    public GameObject bullet;
    public GameObject start;
    public float fireRate;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void Update () {
        transform.rotation = Quaternion.identity;

        if (Input.GetKey(shoot) && last + fireRate < Time.time)
        {
            anim.SetTrigger("shoot");
            Instantiate(bullet, start.transform.position, start.transform.rotation);
            last = Time.time;
        }
    }

	void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontal, vertical) * speed;
        xSpeed = rb.velocity.x;
    }

    public static float GetSpeed()
    {
        return xSpeed;
    }
}
