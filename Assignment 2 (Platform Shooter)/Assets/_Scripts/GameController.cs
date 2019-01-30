using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private static int kitsPresent;
    private static float last;
    private Vector2 spawnPos;
    private Rigidbody2D player1rb;
    private Rigidbody2D player2rb;

    public GameObject kit;
    public float minX, minY;
    public float playerMinX, playerMinY;
    public float spawnRate;
    public GameObject player1;
    public GameObject player2;

    // Use this for initialization
    void Start () {
        kitsPresent = 0;
        player1rb = player1.GetComponent<Rigidbody2D>();
        player2rb = player2.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if(kitsPresent == 0 && last + spawnRate < Time.time)
        {
            Vector2 pos;
            while(true)
            {
                pos = new Vector2(Random.Range(minX, -minX), Random.Range(minY, -minY));
                if (Physics2D.OverlapCircleAll(pos, 1).Length < 1)
                    break;
            }
            Instantiate(kit, pos, Quaternion.identity);
            kitsPresent++;
        }
        EndRound();
    }

    public static void KitTaken()
    {
        kitsPresent--;
        last = Time.time;
    }

    private void EndRound()
    {
        if(player1.GetComponent<PlayerController>().health <= 0)
        {
            player1.GetComponent<PlayerController>().health = 100;
            player2.GetComponent<PlayerController>().health = 100;
            player2.GetComponent<PlayerController>().score += 1;
            RespawnPlayers();
        }else if(player2.GetComponent<PlayerController>().health <= 0)
        {
            player1.GetComponent<PlayerController>().health = 100;
            player2.GetComponent<PlayerController>().health = 100;
            player1.GetComponent<PlayerController>().score += 1;
            RespawnPlayers();
        }
    }

    private void RespawnPlayers()
    {
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }

        player1rb.transform.position = new Vector2(Random.Range(playerMinX, 0), Random.Range(playerMinY, 0));
        player2rb.transform.position = new Vector2(Random.Range(0, -playerMinX), Random.Range(0, -playerMinY));
    }
}
