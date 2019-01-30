using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {

    public Canvas menu;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Pause();		
	}

    private void Pause()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !menu.gameObject.activeInHierarchy)
        {
            Time.timeScale = 0;
            menu.gameObject.SetActive(true);
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
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenu");
    }
}
