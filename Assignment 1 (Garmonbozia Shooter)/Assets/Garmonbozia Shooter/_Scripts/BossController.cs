using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public GameObject explosion, bullet, start;
    public float dropSpeed;
    public float speed;
    public int lives;
    public float yMin, yMax;

    private Rigidbody2D rb;
    private bool hasExploded;
    private float fireRate, minRate, maxRate, last, ySpeed;
    private int livesLeft;


    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ySpeed = speed;
        rb.velocity = new Vector2(-speed, ySpeed);

        hasExploded = false;
        minRate = 1f;
        maxRate = 2f;
        fireRate = 0;
        livesLeft = lives;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.identity;
        if (last + fireRate < Time.time && !hasExploded)
        {
            Instantiate(bullet, start.transform.position, start.transform.rotation);
            last = Time.time;
            fireRate = Random.Range(minRate, maxRate);
        }
    }

    void FixedUpdate() {
//        rb.velocity = new Vector2(-speed, Mathf.Sin(Time.time));
        if((transform.position.y < yMin || transform.position.y > yMax) && !hasExploded)
        {
            ySpeed = -ySpeed;
            rb.velocity = new Vector2(-speed, ySpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        } else if (coll.gameObject.tag == "PlayerBullet")
        {
            if (livesLeft == 0)
            {
                if (!hasExploded)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                    hasExploded = true;
                    GameController.UpdateEnemies();
                }
                rb.velocity = new Vector2(0, -dropSpeed);
            } else
            {
                livesLeft--;
                rb.velocity = new Vector2(-speed, ySpeed);
            }
        } else if (coll.gameObject.tag == "WestWall" || coll.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameController.GameOver();
        }
    }
}
