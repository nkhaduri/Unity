using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] standartEnemies;
    public GameObject[] bosses;
    public int numEnemies;
    public float roundWait, waitTime, x, minY, maxY, bossMinY, bossMaxY;

    private int roundNum;
    private static int enemiesLeft;
    private static bool isGameOver;
    private GameObject[] toDestroy;

    // Use this for initialization
    void Start () {
        Restart();
	}

    IEnumerator Rounds() {
        yield return new WaitForSeconds(roundWait);
        roundNum++;
        for (int i = 0; i < numEnemies; i++)
        {
            if (roundNum % 3 == 0 && i == numEnemies - 1)
            {
                Instantiate(bosses[Random.Range(0, bosses.Length)],
                    new Vector2(x, Random.Range(bossMinY, bossMaxY)), Quaternion.identity);
                break;
            }
            Instantiate(standartEnemies[Random.Range(0, standartEnemies.Length)],
                new Vector2(x, Random.Range(minY, maxY)), Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
        numEnemies++;
    }

    public static void UpdateEnemies() {
        enemiesLeft--;
    }

    public static void GameOver() {
        isGameOver = true;
    }

    private void Restart() {
        roundNum = 0;
        isGameOver = false;
        numEnemies = 2;

        enemiesLeft = numEnemies;
        StartCoroutine(Rounds());
    }

    // Update is called once per frame
    void Update () {
        if(isGameOver)
        {
            SceneManager.LoadScene("Assignment1");
        }
        if (enemiesLeft == 0)
        {
            enemiesLeft = numEnemies;
            StartCoroutine(Rounds());
        }
            
	}
}
