using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunController: GameManager.GameStateController
{
    // Character
    public GameObject character;
    public MoveController moveController;

    // Camera
    public Transform mainCameraTransform;

    // Run status
    public GameManager.RunStatus runStatus;

    // Road
    public string roadPrefabPath;
    public int roadDistance;
    public int maxExistRoad;

    private Queue<GameObject> roadQueue = new Queue<GameObject>();
    private GameObject[] roadPrefabArray;

    // Background Music
    public string[] backgroundMusicPath;
    private int currentMusicIndex;

    // Skybox
    public enum MySky
    {
        Morning = 0,
        Day,
        Dusk,
        Night,
        Rain,
        Snow
    }
    private int curSky;
    public Material[] skyBox;
    public GameObject snowSky;
    public GameObject rainSky;
    private int GetNewSky()
    {
        int newSky = -1;

        if(curSky == (int)MySky.Rain || curSky == (int)MySky.Snow)
        {
            curSky = (int)MySky.Day;
        }
        switch (curSky)
        {
            case (int)MySky.Morning:
                newSky = Random.value < 0.7 ? (int)MySky.Day : (Random.value < 0.85 ? (int)MySky.Rain : (int)MySky.Snow);
                break;
            case (int)MySky.Day:
                newSky = (int)MySky.Dusk;
                break;
            case (int)MySky.Dusk:
                newSky = (int)MySky.Night;
                break;
            case (int)MySky.Night:
                    newSky = (int)MySky.Morning;
                break;
        }
        return newSky;
    }

    // GUI
    private Rect rctScore;
    private Rect rctHealthBar;

    // Use this for initialization
    void Start()
    {
        // Initialize roads.
        roadPrefabArray = Resources.LoadAll<GameObject>(roadPrefabPath);

        // GUI
        rctScore = new Rect(50, 50, 150, 40);
        rctHealthBar = new Rect(Screen.width / 2 - 100, 20, 200, 20);

        runStatus = GetRunStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (newlyEnabled)
        {
            newlyEnabled = false;

            // Initialize camera.
            UpdateCameraTransform(mainCameraTransform);

            // Initialize roads.
            for (int i = 0; i < maxExistRoad; i++)
            {
                GameObject newRoad = roadPrefabArray[Random.Range(0, roadPrefabArray.Length)];     // Randomly choose a prefab
                newRoad = Instantiate(newRoad,
                    new Vector3(0, 0, (i * roadDistance)),
                    this.transform.rotation);

                int isRotate = Random.value > 0.5 ? 1 : 0;
                newRoad.transform.Rotate(0, isRotate * 180, 0);
                curSky = newRoad.GetComponent<ObstacleInitializer>().sky = GetNewSky();

                roadQueue.Enqueue(newRoad);
            }

            // Initialize background music.
            SetBackgroundMusicPath(backgroundMusicPath[(int)MySky.Morning]);

            // Initialize MoveController.
            moveController = character.GetComponent<MoveController>();
            moveController.runStatus = GetRunStatus();
            moveController.SetLastTouchedRoad(null);
            moveController.modeSpeed = GetModeSpeed();
            moveController.SetIsFailed(false);
            moveController.enabled = true;

            // Initialize Sky.
            curSky = (int)MySky.Morning;
        }

        if (moveController.GetNeedNewSky())
        {
            moveController.SetNeedNewSky(false);

            // Skybox.
            int newSky = moveController.GetCurTouchedSky();
            RenderSettings.skybox = skyBox[newSky];
            DynamicGI.UpdateEnvironment();

            // Particle system.
            if (newSky == (int)RunController.MySky.Rain)
            {
                rainSky.SetActive(true);
                snowSky.SetActive(false);
            }
            else if (newSky == (int)RunController.MySky.Snow)
            {
                rainSky.SetActive(false);
                snowSky.SetActive(true);
            }
            else
            {
                snowSky.SetActive(false);
                rainSky.SetActive(false);
            }

            // Background Music.
            SetBackgroundMusicPath(backgroundMusicPath[newSky]);
        }

        UpdateBackgroundMusic();
        if (moveController.GetNeedNewRoad())
        {
            moveController.SetNeedNewRoad(false);
            CreateNewRoad();
        }

        if (moveController.GetIsFailed())
        {
            SetGameState((int)GameManager.GameState.Stop);
        }
    }

    // Destroy
    public override void AfterStop()
    {
        SetKinectActivate(false);
        moveController.enabled = false;
        foreach (GameObject x in roadQueue)
        {
            Destroy(x);
        }
    }

    // Change the position of road behind the player
    public void CreateNewRoad()
    {
        GameObject oldRoad = roadQueue.Dequeue();

        GameObject newRoad = roadPrefabArray[Random.Range(0, roadPrefabArray.Length)];     // Randomly choose a prefab
        newRoad = Instantiate(newRoad);

        newRoad.transform.position = new Vector3(0, 0,
            oldRoad.transform.position.z + (maxExistRoad * roadDistance));
        int isRotate = Random.value > 0.5 ? 1 : 0;
        newRoad.transform.Rotate(0, isRotate * 180, 0);
        curSky = newRoad.GetComponent<ObstacleInitializer>().sky = GetNewSky();
        Debug.Log(newRoad.GetComponent<ObstacleInitializer>().sky);

        roadQueue.Enqueue(newRoad);

        Destroy(oldRoad, 3);
    }

    private void OnGUI()
    {
        GUI.skin = GetGUISkin();
        GUI.Box(rctScore, "Score " + runStatus.GetTotalScore());

        //GUI.HorizontalScrollbar(rctHealthBar, runStatus.GetHealth(), 100, 0, runStatus.GetHealth());
    }
}
