using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSettingController : GameManager.GameStateController
{
    // GUI
    private Rect rctBackground;
    private Rect rctTitle;
    private Rect rctBackButton;

    private Rect rctEasyButton;
    private Rect rctMediumButton;
    private Rect rctHardButton;
    private Rect rctImpossibleButton;

    // Use this for initialization
    void Start () {
        // GUI
        rctBackground = new Rect(Screen.width / 2 - 200, 50, Screen.width / 2 + 180, Screen.height - 80);
        rctTitle = new Rect(Screen.width / 2 + 20, 75, 532, 103);
        rctBackButton = new Rect(Screen.width / 2 - 180, 95, 185, 67);

        rctEasyButton = new Rect(Screen.width / 2 + 130, Screen.height - 300, 126, 68);
        rctMediumButton = new Rect(Screen.width / 2 + 80, Screen.height - 240, 219, 65);
        rctHardButton = new Rect(Screen.width / 2 + 120, Screen.height - 180, 155, 66);
        rctImpossibleButton = new Rect(Screen.width / 2 + 65, Screen.height - 120, 253, 66);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateBackgroundMusic();
    }

    private void OnGUI()
    {
        GUI.skin = GetGUISkin();
        GUI.Box(rctBackground, "", "Background");
        GUI.Box(rctTitle, "", "ModeTitle");

        if (GUI.Button(rctEasyButton, "", "ModeEasy"))
        {
            SetMode((int)GameManager.GameMode.Easy);
            SetGameState((int)GameManager.GameState.KinectUserDitect);
        }
        else if (GUI.Button(rctMediumButton, "", "ModeMedium"))
        {
            SetMode((int)GameManager.GameMode.Medium);
            SetGameState((int)GameManager.GameState.KinectUserDitect);
        }
        else if (GUI.Button(rctHardButton, "", "ModeHard"))
        {
            SetMode((int)GameManager.GameMode.Hard);
            SetGameState((int)GameManager.GameState.KinectUserDitect);
        }
        else if (GUI.Button(rctImpossibleButton, "", "ModeImpossible"))
        {
            SetMode((int)GameManager.GameMode.Impossible);
            SetGameState((int)GameManager.GameState.KinectUserDitect);
        }
        else if (GUI.Button(rctBackButton, "", "BackButton"))
        {
            SetGameState((int)GameManager.GameState.Start);
        }
    }
}
