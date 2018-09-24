using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game State 
    public enum GameState
    {
        OpeningAnimation = 0,
        Start,
        ModeSetting,
        KinectUserDitect,
        Run,
        Stop,
        Setting
    };
    private static int gameState;
   
    // Camera
    public Camera mainCamera;

    // Background Music
    public AudioSource backgroundMusic;

    private static AudioClip[] backgroundMusicArray;
    private static int currentMusicIndex;
    private static float backgroundMusicVolume;

    // GameMode
    public enum GameMode
    {
        Easy = 101,
        Medium,
        Hard,
        Impossible
    }
    public float[] modeSpeed; 
    private static int mode;

    // GUI
    public GUISkin guiskin;

    // Kinect
    public GameObject kinectController;
    private static string curGesture;
    private static bool isUserExist;

    // Run status
    public class RunStatus
    {
        public const int maxHealth = 1000;

        private float curSpeed;
        private int totalScore;
        private int loseScore;
        private int bonusScore;
        private int health;

        public RunStatus()
        {
            curSpeed = totalScore = loseScore = bonusScore = 0;
            health = maxHealth;
        }
        public RunStatus(RunStatus rs)
        {
            this.curSpeed = rs.curSpeed;
            this.totalScore = rs.totalScore;
            this.loseScore = rs.loseScore;
            this.bonusScore = rs.bonusScore;
            this.health = rs.health;
        }

        public void SetScoresHealth(int totalScore, int loseScore, int bonusScore, int health)
        {
            this.totalScore = totalScore;
            this.loseScore = loseScore;
            this.bonusScore = bonusScore;
            this.health = health;
        }
        public void SetCurSpeed(float speed)
        {
            float newSpeed = 0.4f * this.curSpeed + 0.6f * speed;
            if (newSpeed > 1.9f) newSpeed = 1.9f;
            this.curSpeed = newSpeed;
        }

        public float GetCurSpeed()
        {
            return curSpeed;
        }
        public int GetTotalScore()
        {
            return totalScore;
        }
        public int GetLoseScore()
        {
            return loseScore;
        }
        public int GetBonusScore()
        {
            return bonusScore;
        }
        public int GetHealth()
        {
            return health;
        }
    }
    private RunStatus runStatus;

    // Game State Controller
    public class GameStateController : MonoBehaviour
    {
        public GameManager gm;

        // Enable control
        public bool newlyEnabled = false;

        // Game State
        public int GetGameState()
        {
            return gameState;
        }

        public void SetGameState(int state)
        {
            gameState = state;
            gm.OnGameStateChanged();
        }

        public virtual void AfterStop() { }

        // Camera
        public Transform GetCameraTransform()
        {
            return gm.mainCamera.transform;
        }

        public void UpdateCameraTransform(Transform transform)
        {
            gm.mainCamera.transform.position = new Vector3(
                Mathf.Lerp(gm.mainCamera.transform.position.x, transform.position.x, 5.0f * Time.deltaTime),
                Mathf.Lerp(gm.mainCamera.transform.position.y, transform.position.y, 5.0f * Time.deltaTime),
                Mathf.Lerp(gm.mainCamera.transform.position.z, transform.position.z, 5.0f * Time.deltaTime));

            gm.mainCamera.transform.rotation = new Quaternion(
                Mathf.Lerp(gm.mainCamera.transform.rotation.x, transform.rotation.x, Time.deltaTime),
                Mathf.Lerp(gm.mainCamera.transform.rotation.y, transform.rotation.y, Time.deltaTime),
                Mathf.Lerp(gm.mainCamera.transform.rotation.z, transform.rotation.z, Time.deltaTime),
                Mathf.Lerp(gm.mainCamera.transform.rotation.w, transform.rotation.w, Time.deltaTime));
        }

        // Background music
        public void SetBackgroundMusicVolume(float value)
        {
            backgroundMusicVolume = value;
        }   

        public void SetBackgroundMusicPath(string path)
        {
            backgroundMusicArray = Resources.LoadAll<AudioClip>(path);

            currentMusicIndex = (currentMusicIndex + 1) % backgroundMusicArray.Length;
            gm.backgroundMusic.clip = backgroundMusicArray[currentMusicIndex];
            gm.backgroundMusic.Play();
        }

        public void UpdateBackgroundMusic()
        {
            // Background music.
            gm.backgroundMusic.volume = backgroundMusicVolume; // Update the volume.

            if (gm.backgroundMusic != null & !gm.backgroundMusic.isPlaying)    // Next song.
            {
                currentMusicIndex = (currentMusicIndex + 1) % backgroundMusicArray.Length;
                gm.backgroundMusic.clip = backgroundMusicArray[currentMusicIndex];
                gm.backgroundMusic.Play();
            }
        }

        // GUI
        public GUISkin GetGUISkin()
        {
            return gm.guiskin;
        }

        // Mode
        public void SetMode(int value)
        {
            mode = value;
        }
        public float GetModeSpeed()
        {
            return gm.modeSpeed[mode - (int)GameMode.Easy];
        }

        // Kinect
        public void SetKinectActivate(bool value)
        {
            gm.kinectController.SetActive(value);
        }
        public string GetCurGesture()
        {
            return curGesture;
        }
        public bool GetIsUserExist()
        {
            return isUserExist;
        }

        // Run Status
        public RunStatus GetRunStatus()
        {
            return gm.runStatus;
        }
    }
    private GameStateController curController;
    private GameStateController backController;

    // Use this for initialization
    void Start()
    {
        //Resolution[] resolutions = Screen.resolutions;
        //Screen.SetResolution(
        //    resolutions[resolutions.Length - 1].width,
        //    resolutions[resolutions.Length - 1].height,
        //    true);
        //
        //Screen.fullScreen = true;

        gameState = (int)GameState.Start;
        OnGameStateChanged();
        runStatus = new RunStatus();
        isUserExist = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Game State changed.
    void OnGameStateChanged()
    {
        switch (gameState)
        {
            case (int)GameState.Start:
                if (curController != null)
                {
                    curController.enabled = false;
                }
                curController = GetComponent<StartController>();
                curController.newlyEnabled = true;
                curController.enabled = true;
                break;
            case (int)GameState.ModeSetting:
                curController.enabled = false;
                curController = GetComponent<ModeSettingController>();
                curController.newlyEnabled = true;
                curController.enabled = true;
                break;
            case (int)GameState.KinectUserDitect:
                curController.enabled = false;
                curController = GetComponent<KinectDetectController>();
                curController.newlyEnabled = true;
                curController.enabled = true;
                break;
            case (int)GameState.Run:
                if (curController != null)
                {
                    curController.enabled = false;
                }
                if (backController != null)
                {
                    curController = backController;
                    backController = null;
                    curController.enabled = true;
                }
                else
                {
                    curController = GetComponent<RunController>();
                    curController.newlyEnabled = true;
                    curController.enabled = true;
                }
                break;
            case (int)GameState.Stop:
                curController.AfterStop();
                curController.enabled = false;
                curController = GetComponent<StopController>();
                curController.newlyEnabled = true;
                curController.enabled = true;
                break;
        }
    }

    // Kinect Gesture Listener: Set current Gesture.
    public void SetIsUserExist(bool value)
    {
        isUserExist = value;
    }
    public void SetCurGesture(string gesture)
    {
        curGesture = gesture;
    }
    public void SetCurProgress(float value)
    {
        // Define your rule between progress and speed here.
        runStatus.SetCurSpeed(value);
    }
}
