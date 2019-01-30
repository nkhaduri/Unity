using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * (-speed);
    }

    // Update is called once per frame
    void Update() {
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "EastWall" || coll.gameObject.tag == "WestWall")
        {
            Destroy(gameObject);
        } else if (coll.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameController.GameOver();
        }
    }
}
