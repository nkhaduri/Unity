using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    public Canvas menu;
    public int numTanks;
    public Text scoreText;

    [HideInInspector] public int[] scores;

    // Use this for initialization
    void Start()
    {
        scores = new int[numTanks];
        for (int i = 0; i < numTanks; i++)
            scores[i] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    private void Pause()
    {
        if (!menu.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                scoreText.text = "Current Score: " + scores[0];
                menu.gameObject.SetActive(true);
            } else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Time.timeScale = 0;
                scoreText.text = "Current Score: " + scores[1];
                menu.gameObject.SetActive(true);
            }
        }
    }

    public void Resume()
    {
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CompleteMainScene");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
