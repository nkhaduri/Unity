using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    private Rigidbody2D rb;
    private float startX;

    public AudioClip hitClip;

    // Use this for initialization
    void Start()
    {
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "World")
        {
            Destroy(gameObject);
        }  else if (coll.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            SoundManager.instance.Play(hitClip);
            float endX = transform.position.x;
            if(Mathf.Abs(endX - startX) <= 1)
            {
                coll.gameObject.GetComponent<PlayerController>().health -= 100;
            } else if(Mathf.Abs(endX - startX) <= 5)
            {
                coll.gameObject.GetComponent<PlayerController>().health -= 50;
            } else
            {
                coll.gameObject.GetComponent<PlayerController>().health -= 25;
            }
        }
    }
}
