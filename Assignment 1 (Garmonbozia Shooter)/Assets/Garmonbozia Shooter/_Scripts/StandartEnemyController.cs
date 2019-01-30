using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartEnemyController : MonoBehaviour {

    public GameObject explosion, bullet, start;
    public float dropSpeed;
    public float speed;

    private Rigidbody2D rb;
    private bool hasExploded;
    private float fireRate, minRate, maxRate, last;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * (-speed);
        hasExploded = false;
        minRate = 1.5f;
        maxRate = 2.5f;
        fireRate = 0;
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

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        } else if (coll.gameObject.tag == "PlayerBullet")
        {
            if (!hasExploded)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                hasExploded = true;
                GameController.UpdateEnemies();
            }
            rb.velocity = new Vector2(0, -dropSpeed);
        } else if (coll.gameObject.tag == "WestWall" || coll.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameController.GameOver();
        }
    }
}
