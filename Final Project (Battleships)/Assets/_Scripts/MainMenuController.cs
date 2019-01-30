using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public GameObject menu, options, highscores;
    public Text[] highscoreTexts;
    public Slider volumeSlider;

    private int[] scores;

    public void Play()
    {
        SceneManager.LoadScene("PlaceShipsScene");
    }

    public void Options()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void Highscores()
    {
        menu.SetActive(false);
        highscores.SetActive(true);
        scores = HighScoreManager.instance.highscores;
        for(int i = 0; i < 5; i++)
        {
            highscoreTexts[i].text = i + ". " + scores[i];
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsBack()
    {
        menu.SetActive(true);
        options.SetActive(false);
    }

    public void HighscoresBack()
    {
        menu.SetActive(true);
        highscores.SetActive(false);
    }

    public void ChangeVolume(float v)
    {
        SoundManager.instance.aSource.volume = v;
    }

    // Use this for initialization
    void Start () {
        volumeSlider.value = SoundManager.instance.aSource.volume;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
