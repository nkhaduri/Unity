using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public Text player1BadName, player2BadName, sameNamesOrColors;
    public Dropdown roundsToWin, player1Colour, player2Colour;
    public InputField player1Name, player2Name;

    private GameObject playerInfos;

    [HideInInspector] public List<Color> colours;

    private string name1, name2;

    // Use this for initialization
    void Start () {
        playerInfos = GameObject.Find("PlayerInfos");
        roundsToWin.value = 9;
        player1Colour.value = 3;
        player2Colour.value = 2;
        colours = new List<Color>() { Color.white, Color.black, Color.red, Color.blue, Color.yellow, Color.green, Color.grey};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartButton()
    {
        name1 = player1Name.text;
        name2 = player2Name.text;
        bool flag = false;
        if (!IsValid(name1))
        {
            flag = true;
            player1BadName.gameObject.SetActive(true);
        } else
        {
            player1BadName.gameObject.SetActive(false);
        }

        if (!IsValid(name2))
        {
            flag = true;
            player2BadName.gameObject.SetActive(true);
        } else
        {
            player2BadName.gameObject.SetActive(false);
        }


        if((name1 == name2 && name1.Length != 0) || colours[player1Colour.value] == colours[player2Colour.value])
        {
            flag = true;
            sameNamesOrColors.gameObject.SetActive(true);
        } else
        {
            sameNamesOrColors.gameObject.SetActive(false);
        }

        if(!flag)
        {
            playerInfos.GetComponent<PlayerInfoManager>().playerNames = new string[2];
            playerInfos.GetComponent<PlayerInfoManager>().playerColors = new Color[2];
            playerInfos.GetComponent<PlayerInfoManager>().playerColors[0] = colours[player1Colour.value];
            playerInfos.GetComponent<PlayerInfoManager>().playerColors[1] = colours[player2Colour.value];
            playerInfos.GetComponent<PlayerInfoManager>().playerNames[0] = name1;
            playerInfos.GetComponent<PlayerInfoManager>().playerNames[1] = name2;
            playerInfos.GetComponent<PlayerInfoManager>().roundsToWin = roundsToWin.value + 1;
            SceneManager.LoadScene("CompleteMainScene");
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private bool IsValid(string name)
    {
        if (name.Length == 0)
            return true;

        if (name[0] == ' ')
            return false;

        int lettersAndDigits = 0;
        for(int i = 0; i < name.Length; i++)
        {
            if ((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z') || (name[i] >= '0' && name[i] <= '9'))
                lettersAndDigits++;
            else if (name[i] != ' ' && name[i] != '_')
                return false;
        }

        if (lettersAndDigits < 3)
            return false;

        return true;
    }
}
