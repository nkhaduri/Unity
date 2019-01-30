using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour {

    private GameObject gameController;

    [Command]
    void CmdShareGrid(bool[] grid)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                gameController.GetComponent<GameController>().enemyShips[i, j] = grid[i * 10 + j];
            }
        }

        gameController.GetComponent<GameController>().isOpponentReady = true;
    }

    [ClientRpc]
    void RpcShareGrid(bool[] grid)
    {
        if(!isServer)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameController.GetComponent<GameController>().enemyShips[i, j] = grid[i * 10 + j];
                }
            }
            gameController.GetComponent<GameController>().myTurn = true;
            gameController.GetComponent<GameController>().isOpponentReady = true;
        }
    }

    [Command]
    void CmdShareFire(int[] coord)
    {
        if (!gameController.GetComponent<GameController>().mineHit[coord[0], coord[1]])
        {
            if (gameController.GetComponent<GameController>().isShip[coord[0], coord[1]])
            {
                Instantiate(gameController.GetComponent<GameController>().hitTile,
                    gameController.GetComponent<GameController>().myBoard.GetComponent<CreateBoard>().
                    tileObjects[coord[0], coord[1]].transform.position,
                    Quaternion.identity);
            }
            else
            {
                Instantiate(gameController.GetComponent<GameController>().missTile,
                    gameController.GetComponent<GameController>().myBoard.GetComponent<CreateBoard>().
                    tileObjects[coord[0], coord[1]].transform.position,
                    Quaternion.identity);
                gameController.GetComponent<GameController>().myTurn = true;
            }
            gameController.GetComponent<GameController>().mineHit[coord[0], coord[1]] = true;
        }
    }

    [ClientRpc]
    void RpcShareFire(int[] coord)
    {
        if (!isServer)
        {
            if (!gameController.GetComponent<GameController>().mineHit[coord[0], coord[1]])
            {
                if (gameController.GetComponent<GameController>().isShip[coord[0], coord[1]])
                {
                    Instantiate(gameController.GetComponent<GameController>().hitTile, 
                        gameController.GetComponent<GameController>().myBoard.GetComponent<CreateBoard>().
                        tileObjects[coord[0], coord[1]].transform.position,
                        Quaternion.identity);
                } else
                {
                    Instantiate(gameController.GetComponent<GameController>().missTile,
                        gameController.GetComponent<GameController>().myBoard.GetComponent<CreateBoard>().
                        tileObjects[coord[0], coord[1]].transform.position,
                        Quaternion.identity);
                    gameController.GetComponent<GameController>().myTurn = true;
                }
                gameController.GetComponent<GameController>().mineHit[coord[0], coord[1]] = true;
            }
        }
    }

    [Command]
    void CmdShareGameOver(bool won)
    {
        gameController.GetComponent<GameController>().GameOver(!won);
    }

    [ClientRpc]
    void RpcShareGameOver(bool won)
    {
        if (!isServer)
        {
            gameController.GetComponent<GameController>().GameOver(!won);
        }
    }

    public void Done(bool[] grid)
    {
        if (isServer)
            RpcShareGrid(grid);
        else
            CmdShareGrid(grid);
    }

    public void Fire(int[] coord)
    {
        if (isServer)
            RpcShareFire(coord);
        else
            CmdShareFire(coord);
    }

    public void GameOver(bool won)
    {
        if (isServer)
            RpcShareGameOver(won);
        else
            CmdShareGameOver(won);
    }

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer)
            gameObject.SetActive(false);
        gameController = GameObject.Find("GameController");
    }
	
	// Update is called once per frame
	void Update () {

	}
}
