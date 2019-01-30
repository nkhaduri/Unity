using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float last;
    private Animator anim;
    private bool facingEast;
    private GameObject b;
    private Rigidbody2D bulletBody;
    private float h;

    public KeyCode shoot;
    public KeyCode right;
    public KeyCode left;
    public KeyCode jump;
    public KeyCode run;

    public float walkSpeed;
    public float runSpeed;
    public float bulletSpeed;
    public GameObject bullet;
    public GameObject start;
    public float fireRate;
    public bool initiallyFacingEast;
    public Transform bottomPoint;
    public float jumpForce;
    public LayerMask platforms;
    public GameObject gunFireParticle;
    public Transform teleportRespawn;
    public bool isPlayer1;
    public int health;
    public int score;
    public Text healthT, scoreT;
    public AudioClip shootClip;
    public AudioClip jumpClip;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingEast = initiallyFacingEast;
    }
	
	// Update is called once per frame
	void Update () {

        healthT.text = "Health: " + health.ToString();
        scoreT.text = "Score: " + score.ToString();

        if (Input.GetKey(shoot) && last + fireRate < Time.time)
        {
            anim.SetTrigger("shoot");
            SoundManager.instance.Play(shootClip);
            b = Instantiate(bullet, start.transform.position, start.transform.rotation);
            Instantiate(gunFireParticle, start.transform.position, start.transform.rotation);
            bulletBody = b.GetComponent<Rigidbody2D>();
            if(!facingEast)
            {
                bulletBody.transform.localScale = new Vector3(-bulletBody.transform.localScale.x, bulletBody.transform.localScale.y,
                        bulletBody.transform.localScale.z);
                bulletBody.velocity = transform.right * (-bulletSpeed);
            } else
            {
                bulletBody.velocity = transform.right * bulletSpeed;
            }
            last = Time.time;
        }
    }

    void FixedUpdate() {
        if (Input.GetKey(jump))
        {
            if (IsOnGround())
            {
                anim.SetTrigger("jump");
                SoundManager.instance.Play(jumpClip);
                rb.AddForce(new Vector2(0, jumpForce));
            }
        }

        if(isPlayer1)
            h = Input.GetAxis("Horizontal");
        else
            h = Input.GetAxis("Horizontal1");
        rb.velocity = new Vector2(h * walkSpeed, rb.velocity.y);
        TurnAround(h);
        anim.SetBool("walk", h != 0);
        
    }

    private void TurnAround(float h)
    {
        if((h > 0 && !facingEast) || (h < 0 && facingEast)) 
        {
            facingEast = !facingEast;

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
        }
    }

    private bool IsOnGround()
    {
        if(rb.velocity.y <= 0)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(bottomPoint.position, 0.05f, platforms);
            for(int i = 0; i < coll.Length; i++)
            {
                if (coll[i].gameObject != gameObject)
                    return true;
            }
        }
        return false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Teleport")
        {
            transform.position = teleportRespawn.position;
        } else if (coll.gameObject.tag == "Health")
        {
            Destroy(coll.gameObject);
            GameController.KitTaken();
            health = 100;
        }
    }
}
