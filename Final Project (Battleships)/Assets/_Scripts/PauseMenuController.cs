using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {

    public GameObject pause_menu, canvas;
    public Slider volumeSlider;

    [HideInInspector]
    public bool paused = false;

    public void ChangeVolume(float v)
    {
        SoundManager.instance.aSource.volume = v;
    }

    public void Pause()
    {
        pause_menu.SetActive(true);
        canvas.SetActive(false);
        Time.timeScale = 0;
        paused = true;
    }

    public void Resume()
    {
        pause_menu.SetActive(false);
        canvas.SetActive(true);
        Time.timeScale = 1;
        paused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Use this for initialization
    void Start () {
        volumeSlider.value = SoundManager.instance.aSource.volume;
    }
	
	// Update is called once per frame
	void Update () {
	}
}
