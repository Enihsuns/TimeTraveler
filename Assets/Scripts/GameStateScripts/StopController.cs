using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : GameManager.GameStateController
{
    // GUI
    Rect rctBackground;
    Rect rctScoreText;
    Rect rctLoseScoreText;
    Rect rctTryButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (GetGameState() == (int)GameManager.GameState.Stop)
        {
            GUI.Box(rctBackground,"","Background");
            GUI.Box(rctScoreText, "Score" + GetRunStatus().GetTotalScore());
            GUI.Box(rctLoseScoreText, "Hitted Obstacles" + GetRunStatus().GetLoseScore() / 50);

            if(GUI.Button(rctTryButton,"Try again!"))
            {
                SetGameState((int)GameManager.GameState.Start);
            }
        }
    }
}
