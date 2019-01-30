using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragnDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public GameObject gameController;
    public GameObject board;

    private Collider2D coll;
    private BoxCollider2D b_coll;
    private Vector2 pos, pointA, pointB, init_b_coll_size, prev_pos;

    [HideInInspector]
    public int sz, x = -1, y = -1;

    [HideInInspector]
    public bool rotated;

    [HideInInspector]
    public Vector2 init_pos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameController.GetComponent<GameController>().isReady)
            return;
        prev_pos = transform.position;
        b_coll.size = init_b_coll_size;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameController.GetComponent<GameController>().isReady)
            return;
        pos = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameController.GetComponent<GameController>().isReady)
            return;
        pos = transform.position;
        pointA = new Vector2(pos.x + coll.bounds.size.x / 2.0f, pos.y + coll.bounds.size.y / 2.0f);
        pointB = new Vector2(pos.x - coll.bounds.size.x / 2.0f, pos.y - coll.bounds.size.y / 2.0f);
        Collider2D[] colls = Physics2D.OverlapAreaAll(pointA, pointB);
        for(int i = 0; i < colls.Length; i++)
        {
            if(colls[i].gameObject.tag != "tile" && colls[i].gameObject.name != gameObject.name)
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }
        }

        if(!rotated)
        {
            Collider2D[] tiles = Physics2D.OverlapCircleAll(new Vector2(pos.x - coll.bounds.size.x / 2.0f, pos.y), 0.05f);
            if(tiles.Length == 0 || (tiles.Length == 1 && tiles[0].gameObject.name == gameObject.name))
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }
            Collider2D tile = tiles[0];
            for (int i = 0; i < tiles.Length; i++)
            {
                if(tiles[i].gameObject.tag == "tile")
                {
                    tile = tiles[i];
                    break;
                }
            }
            if(tile.gameObject.tag != "tile")
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }

            x = board.GetComponent<CreateBoard>().board[tile.gameObject] % 10;
            y = board.GetComponent<CreateBoard>().board[tile.gameObject] / 10;

            transform.position = 
                new Vector2(tile.transform.position.x + sz * (tile.GetComponent<Renderer>().bounds.size.x) / 2,
                tile.transform.position.y);

            if (x + sz > 10)
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }

            transform.position -= new Vector3(0.3f, 0, 0);

            b_coll.size = new Vector2(b_coll.size.x * 3.5f, b_coll.size.y * (sz + 2.5f) / sz);
        } else {
            Collider2D[] tiles = Physics2D.OverlapCircleAll(new Vector2(pos.x, pos.y + coll.bounds.size.y / 2.0f), 0.05f);
            if (tiles.Length == 0 || (tiles.Length == 1 && tiles[0].gameObject.name == gameObject.name))
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }
            Collider2D tile = tiles[0];
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].gameObject.tag == "tile")
                {
                    tile = tiles[i];
                    break;
                }
            }
            if (tile.gameObject.tag != "tile")
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }

            x = board.GetComponent<CreateBoard>().board[tile.gameObject] % 10;
            y = board.GetComponent<CreateBoard>().board[tile.gameObject] / 10;

            transform.position =
                new Vector2(tile.transform.position.x,
                tile.transform.position.y - sz * (tile.GetComponent<Renderer>().bounds.size.y) / 2);

            if (y + sz > 10)
            {
                if (Mathf.Approximately(prev_pos.x, init_pos.x) && Mathf.Approximately(prev_pos.y, init_pos.y))
                {
                    x = -1;
                    y = -1;
                }
                gameObject.transform.position = prev_pos;
                return;
            }

            transform.position += new Vector3(0, 0.5f, 0);

            b_coll.size = new Vector2(b_coll.size.x * 3.5f, b_coll.size.y * (sz + 2.5f) / sz);
        }
    }

    private void OnMouseDown()
    {
        GameController.lastClicked = gameObject;
    }

    // Use this for initialization
    void Start () {
        init_pos = transform.position;
        coll = GetComponent<Collider2D>();
        b_coll = GetComponent<BoxCollider2D>();
        init_b_coll_size = b_coll.size;
        sz = gameObject.name[gameObject.name.Length - 1] - '0';
        rotated = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
