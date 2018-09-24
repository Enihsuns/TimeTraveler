using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {
    // Kinect Sensitivity
    public float xSens;
    public float ySens;
    public float zSens;

    // Gravity
    public float gravity;

    // Camera
    public float cameraXSpeed;
    public float cameraYSpeed;
    public float cameraZSpeed;
    public float cameraDistance;
    public Transform mainCameraTransform;
    public float modeSpeed;

    // Road control
    private GameObject lastTouchedRoad;
    private bool needNewRoad;

    // Obstacles
    private GameObject lastHitObastacle;

    // Sky
    public int curSky;
    public bool needNewSky;

    // Run status
    public GameManager.RunStatus runStatus;
    private int loseScore;
    private int bonusScore;
    private int health = 1000;
    private bool isFailed;

    // Audio Effect
    public AudioSource audioEffectSource;
    public AudioClip audioEffectClip;

    // Use this for initialization
    void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
        // Update the position of player
        CharacterController characterController = GetComponent<CharacterController>();
        Vector3 moveDirection = new Vector3(0, 0, runStatus.GetCurSpeed() * zSens);
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        // Update the camera
        mainCameraTransform.position = new Vector3(
            Mathf.Lerp(mainCameraTransform.position.x, this.transform.position.x, cameraXSpeed * Time.deltaTime),
            Mathf.Lerp(mainCameraTransform.position.y, 2.0f + this.transform.position.y, cameraYSpeed * Time.deltaTime),
            Mathf.Lerp(mainCameraTransform.position.z, this.transform.position.z - cameraDistance, cameraZSpeed * Time.deltaTime));


        // Update the score and life value.
        runStatus.SetScoresHealth(
            (int)((characterController.transform.position.z-45)*10) - loseScore + bonusScore,
            loseScore,
            bonusScore,
            health);

        // Check if failed.
        if(health <= 0)
        {
            isFailed = true;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.gameObject.tag.Equals("Road"))
        {
            if(lastTouchedRoad == null)
            {
                lastTouchedRoad = hit.collider.gameObject;
            }
            // Create New Road
            if (!hit.collider.gameObject.Equals(lastTouchedRoad))
            {
                needNewRoad = true;
            }
            lastTouchedRoad = hit.collider.gameObject;

            if(curSky != lastTouchedRoad.GetComponentInParent<ObstacleInitializer>().sky)
            {
                curSky = lastTouchedRoad.GetComponentInParent<ObstacleInitializer>().sky;
                SetNeedNewSky(true);
            }
        }
        else if (hit.collider.gameObject.tag.Equals("Ignore"))
        {

        }
        else
        {
            if (!hit.collider.gameObject.Equals(lastHitObastacle))
            {
                lastHitObastacle = hit.collider.gameObject;
                loseScore += 50;
                audioEffectSource.PlayOneShot(audioEffectClip);
            }
            
        }
    }

    public void SetNeedNewRoad(bool value)
    {
        needNewRoad = value;
    }
    
    public bool GetNeedNewRoad()
    {
        return needNewRoad;
    }

    public int GetCurTouchedSky()
    {
        return curSky;
    }

    public bool GetNeedNewSky()
    {
        return needNewSky;
    }

    public void SetNeedNewSky(bool value)
    {
        needNewSky = value;
    }

    public void SetLastTouchedRoad(GameObject gameObject)
    {
        lastTouchedRoad = gameObject;
    }

    public void SetIsFailed(bool value)
    {
        isFailed = value;
    }

    public bool GetIsFailed()
    {
        return isFailed;
    }
}
