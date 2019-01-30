using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{

    public Text winnerText;

    // Use this for initialization
    void Start()
    {
        GameObject winner = GameObject.Find("GameWinner");
        winnerText.text = winner.GetComponent<GameWinnerManager>().winnerName + " Wins the War";
        winnerText.color = winner.GetComponent<GameWinnerManager>().winnerColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene("CompleteMainScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
