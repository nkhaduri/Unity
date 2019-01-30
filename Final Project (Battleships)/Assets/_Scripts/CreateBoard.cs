using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour {

    public float x0, y0;
    public GameObject tile;
    public GameObject[] number_tiles;
    public GameObject[] letter_tiles;
    public int n;

    [HideInInspector]
    public Dictionary<GameObject, int> board = new Dictionary<GameObject, int>();

    [HideInInspector]
    public GameObject[,] tileObjects = new GameObject[10, 10];

    [HideInInspector]
    public float width;

    private float x, y;

    // Use this for initialization
    void Start () {
        width = (float)tile.GetComponent<Renderer>().bounds.size.x;
        x = x0;
        y = y0;
        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                GameObject t = Instantiate(tile, new Vector2(x, y), Quaternion.identity);
                t.transform.parent = gameObject.transform;
                board.Add(t, i * n + j);
                tileObjects[j, i] = t;
                x += width;
            }
            x = x0;
            y -= width;
        }

        x = x0;
        y = y0 + width + 0.05f;
        for(int i = 0; i < number_tiles.Length; i++)
        {
            GameObject t = Instantiate(number_tiles[i], new Vector2(x, y), Quaternion.identity);
            t.transform.parent = gameObject.transform;
            x += width;
        }

        x = x0 - width - 0.05f;
        y = y0;
        for (int i = 0; i < letter_tiles.Length; i++)
        {
            GameObject t = Instantiate(letter_tiles[i], new Vector2(x, y), Quaternion.identity);
            t.transform.parent = gameObject.transform;
            y -= width;
        }

        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
