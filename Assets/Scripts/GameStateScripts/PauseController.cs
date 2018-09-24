using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : GameManager.GameStateController
{
    // GUI
    private Rect rctBackground;
    private Rect rctPauseTitle;
    private Rect rctPauseBackGroundMusicText;
    private Rect rctPauseBackGroundMusicVolume;
    private Rect rctStopButton;

    // Use this for initialization
    void Start () {
        rctBackground = new Rect(10, 10, 10, 10);
        rctPauseTitle = new Rect(10, 10, 10, 10);
        rctPauseBackGroundMusicText = new Rect(10, 10, 10, 10);
        rctPauseBackGroundMusicVolume = new Rect(10, 10, 10, 10);
        rctStopButton = new Rect(10, 10, 10, 10);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        GUI.Box(rctBackground, "", "Background");
        GUI.Box(rctPauseTitle, "", "PauseTitle");
        //...
        if (GUI.Button(rctStopButton, "", "StopButton"))
        {
            SetGameState((int)GameManager.GameState.Stop);
        }
    }
}
