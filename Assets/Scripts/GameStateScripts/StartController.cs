using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : GameManager.GameStateController
{
    // Camera
    public Transform mainCameraTransform;

    // Background music
    public string backgroundMusicPath;
    private float backgroundMusicVolume;

    // GUI
    private Rect rctBackground;
    private Rect rctTitle;

    private Rect rctStartButton;
    private Rect rctSettingButton;
    
    // Use this for initialization
    void Start () {
        SetBackgroundMusicPath(backgroundMusicPath);
        backgroundMusicVolume = 0.8f;
        SetBackgroundMusicVolume(backgroundMusicVolume);

        rctBackground = new Rect(Screen.width / 2 - 200, 50, Screen.width / 2 + 180, Screen.height - 80);
        rctTitle = new Rect(Screen.width / 2 - 180, 75, 692, 135);

        rctStartButton = new Rect(Screen.width - 300, Screen.height - 240, 154, 66);
        rctSettingButton = new Rect(Screen.width - 300, Screen.height - 150, 192, 67);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCameraTransform(mainCameraTransform);
        UpdateBackgroundMusic();
    }

    private void OnGUI()
    {
        if (GetGameState() == (int)GameManager.GameState.Start)
        {
            GUI.skin = GetGUISkin();
            GUI.Box(rctBackground, "", "Background");
            GUI.Box(rctTitle, "", "StartTitle");

            if (GUI.Button(rctStartButton, "", "StartStartButton"))
            {
                SetGameState((int)GameManager.GameState.ModeSetting);
            }
        }
    }
}
