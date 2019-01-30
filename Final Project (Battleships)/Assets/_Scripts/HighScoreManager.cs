using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from https://www.youtube.com/watch?v=KZuqEyxYZCc 
public class HighScoreManager : MonoBehaviour
{
    const string privateCode = "iK4C9Wjrvk2RcWxNErMFwwXO-e0JVnYU2rRl8JmRGpew", publicCode = "5a870b2239992d09e4d4879f", 
        webURL = "http://dreamlo.com/lb/";

    public static HighScoreManager instance = null;

    [HideInInspector]
    public int[] highscores = new int[5];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddNewScore(int score)
    {
        StartCoroutine(UploadNewScore(score));
    }

    IEnumerator UploadNewScore(int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL("nino") + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            UpdateHighscores();
        }
    }

    public void UpdateHighscores()
    {
        StartCoroutine(DownloadHighscores());
    }

    IEnumerator DownloadHighscores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/0/5");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FillHighscores(www.text);
        } 
    }

    void FillHighscores(string text)
    {
        Debug.Log(text);
        string[] lines = text.Split('\n');
        Debug.Log(lines.Length);
        for(int i = 0; i < lines.Length; i++)
        {
            string[] components = lines[i].Split('|');
            if (components.Length < 2)
                break;
            highscores[i] = int.Parse(components[1]);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
