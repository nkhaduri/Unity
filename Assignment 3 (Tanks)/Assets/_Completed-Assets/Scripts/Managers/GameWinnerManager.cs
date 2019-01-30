using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinnerManager : MonoBehaviour {

    public static GameWinnerManager instance = null;

    [HideInInspector] public string winnerName;
    [HideInInspector] public Color winnerColor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
