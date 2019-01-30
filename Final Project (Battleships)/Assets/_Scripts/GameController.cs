using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] ships;
    public GameObject pauseCanvas, readyCanvas, rotateIcon, filledBoard, myBoard, enemyBoard, hitTile, missTile, pauseMenu, WaitText;
    public AudioClip fireClip, missClip, hitClip, sinkClip;
    public GameObject youWonText, youLostText, resumeButton, pauseMenuController, sinkText;
    public Text scoreText;

    [HideInInspector]
    public bool isReady = false, isOpponentReady = false;

    [HideInInspector]
    public static GameObject lastClicked;

    [HideInInspector]
    public bool[,] enemyShips = new bool[10, 10];

    [HideInInspector]
    public bool myTurn;

    [HideInInspector]
    public bool[,] isShip = new bool[10, 10];

    [HideInInspector]
    public bool[,] mineHit = new bool[10, 10];

    private bool[,] isHit = new bool[10, 10];
    private bool[] toSend = new bool[100];
    private Vector2 clickPos;
    private int x, y, sunkShips, subsequent, score;
    private int[] coord = new int[2];
    private bool playing = false;
    
    public void Ready()
    {
        for(int i = 0; i < ships.Length; i++)
        {
            if (ships[i].GetComponent<DragnDrop>().x == -1 || ships[i].GetComponent<DragnDrop>().y == -1)
                return;
        }

        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i].GetComponent<DragnDrop>().rotated)
            {
                for(int j = 0; j < ships[i].GetComponent<DragnDrop>().sz; j++)
                {
                    isShip[ships[i].GetComponent<DragnDrop>().x, ships[i].GetComponent<DragnDrop>().y + j] = true;
                }
            } else
            {
                for (int j = 0; j < ships[i].GetComponent<DragnDrop>().sz; j++)
                {
                    isShip[ships[i].GetComponent<DragnDrop>().x + j, ships[i].GetComponent<DragnDrop>().y] = true;
                }
            }
        }

        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                toSend[i * 10 + j] = isShip[i, j];
            }
        }

        isReady = true;

        if(!isOpponentReady)
        {
            WaitText.SetActive(true);
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNetwork>().Done(toSend);
    }

    // Use this for initialization
    void Start () {
        sunkShips = 0;
        subsequent = 0;
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (!playing && isReady && isOpponentReady)
        {
            readyCanvas.SetActive(false);
            pauseCanvas.SetActive(true);
            rotateIcon.SetActive(false);
            enemyBoard.SetActive(true);
            WaitText.SetActive(false);
            playing = true;
        }

        if (pauseMenu.GetComponent<PauseMenuController>().paused || !myTurn || !isReady || !isOpponentReady)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(clickPos.x > enemyBoard.GetComponent<CreateBoard>().x0 - enemyBoard.GetComponent<CreateBoard>().width / 2f && 
               clickPos.x < enemyBoard.GetComponent<CreateBoard>().x0 + 9.5f * enemyBoard.GetComponent<CreateBoard>().width &&
               clickPos.y < enemyBoard.GetComponent<CreateBoard>().y0 + enemyBoard.GetComponent<CreateBoard>().width / 2f &&
               clickPos.y > enemyBoard.GetComponent<CreateBoard>().y0 - 9.5f * enemyBoard.GetComponent<CreateBoard>().width)
            {
                sinkText.SetActive(false);
                x = (int) ((clickPos.x - 
                    (enemyBoard.GetComponent<CreateBoard>().x0 - enemyBoard.GetComponent<CreateBoard>().width / 2f)) /
                    enemyBoard.GetComponent<CreateBoard>().width);
                y = (int) ((-clickPos.y + 
                    (enemyBoard.GetComponent<CreateBoard>().y0 + enemyBoard.GetComponent<CreateBoard>().width / 2f)) /
                    enemyBoard.GetComponent<CreateBoard>().width);


                if(enemyShips[x, y])
                {
                    if(isHit[x, y])
                    {
                        
                    } else
                    {
                        SoundManager.instance.Play(fireClip);
                        score += (subsequent + 1) * 20;
                        coord[0] = x;
                        coord[1] = y;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNetwork>().Fire(coord);
                        isHit[x, y] = true;
                        StartCoroutine(ShotEffect(hitClip, hitTile));

                        if (SunkShip())
                        {
                            score += 100;
                            StartCoroutine(ShotEffect(sinkClip, null));
                        }
                    }
                } else
                {
                    if (isHit[x, y])
                    {

                    }
                    else
                    {
                        SoundManager.instance.Play(fireClip);
                        subsequent = 0;
                        coord[0] = x;
                        coord[1] = y;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNetwork>().Fire(coord);
                        isHit[x, y] = true;
                        StartCoroutine(ShotEffect(missClip, missTile));
                        myTurn = false;
                    }
                }
            }
        }

    }

    public void GameOver(bool won)
    {
        pauseMenuController.GetComponent<PauseMenuController>().Pause();
        resumeButton.SetActive(false);
        if(won)
        {
            youWonText.SetActive(true);
        } else
        {
            youLostText.SetActive(true);
        }
    }

    IEnumerator ShotEffect(AudioClip clip, GameObject tile)
    {
        yield return new WaitForSeconds(2);
        SoundManager.instance.Play(clip);
        if (tile != null)
        {
            Instantiate(tile, enemyBoard.GetComponent<CreateBoard>().tileObjects[x, y].transform.position,
                                Quaternion.identity);
            scoreText.text = "Score: " + score;
        }
        else
        {
            sinkText.SetActive(true);
            sunkShips++;
            if (sunkShips == 7)
            {
                scoreText.text = "Score: " + score;
                score += 1000;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNetwork>().GameOver(true);
                GameOver(true);
                HighScoreManager.instance.AddNewScore(score);
            }
        }
    }

    bool SunkShip()
    {
        for(int i = x; i < 10; i++)
        {
            if (!enemyShips[i, y])
                break;

            if (!isHit[i, y])
                return false;
        }

        for (int i = x; i >= 0; i--)
        {
            if (!enemyShips[i, y])
                break;

            if (!isHit[i, y])
                return false;
        }

        for (int i = y; i < 10; i++)
        {
            if (!enemyShips[x, i])
                break;

            if (!isHit[x, i])
                return false;
        }

        for (int i = y; i >= 0; i--)
        {
            if (!enemyShips[x, i])
                break;

            if (!isHit[x, i])
                return false;
        }

        return true;
    }
}
