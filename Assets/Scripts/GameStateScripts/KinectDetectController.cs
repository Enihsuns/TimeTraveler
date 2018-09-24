using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectDetectController : GameManager.GameStateController
{
    // Run status
    private GameManager.RunStatus runStatus;

    // Camera
    public Transform mainCameraTransform;

    // GUI
    enum Instructions
    {
        WaitU = 201,
        WaitRun,
        KeepRun
    }
    private int curinstrIndex;
    private Rect rctInstruction;
    private Rect rctBackButton;

    // Time
    private float lstT;
    private float lstRun;

    // Use this for initialization
    void Start () {
        rctBackButton = new Rect(50, 50, 185, 67);
        rctInstruction = new Rect(Screen.width / 2 - 500, 50, 1000, 100);
        curinstrIndex = (int)Instructions.WaitU;

        runStatus = GetRunStatus();
    }
	
	// Update is called once per frame
	void Update () {
        if (newlyEnabled)
        {
            // Activate Kinect.
            SetKinectActivate(true);
        }

        UpdateCameraTransform(mainCameraTransform);

        switch (curinstrIndex)
        {
            case (int)Instructions.WaitU:
                if(GetIsUserExist() == true)
                {
                    curinstrIndex = (int)Instructions.WaitRun;
                }
                break;
            case (int)Instructions.WaitRun:
                if(GetIsUserExist() == false)
                {
                    curinstrIndex = (int)Instructions.WaitU;
                }
                if (GetCurGesture() == "MyRun" && runStatus.GetCurSpeed() >= GetModeSpeed())
                {
                    lstRun = Time.time;
                    curinstrIndex = (int)Instructions.KeepRun;
                }
                break;
            case (int)Instructions.KeepRun: 
                if(Time.time - lstRun > 3.0f)
                {
                    SetGameState((int)GameManager.GameState.Run);
                }
                break;
        }

	}

    private void OnGUI()
    {
        GUI.skin = GetGUISkin();
        if(GUI.Button(rctBackButton, "", "BackButton"))
        {
            SetGameState((int)GameManager.GameState.ModeSetting);
        }

        switch (curinstrIndex)
        {
            case (int)Instructions.WaitU:
                GUI.Box(rctInstruction, "Please stand in front of Kinect");
                break;
            case (int)Instructions.WaitRun:
                GUI.Box(rctInstruction, "Run! Keep running faster until you reach the minimal speed");
                break;
            case (int)Instructions.KeepRun:
                GUI.Box(rctInstruction, "Ready?");
                break;
        }
    }
}
