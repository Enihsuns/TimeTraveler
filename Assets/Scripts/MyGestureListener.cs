using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface {
    public GameManager gameManager;

    private bool progressDisplayed;
    private float progressGestureTime;
    
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        if (progressDisplayed)
        {
            progressDisplayed = false;

            gameManager.SetCurGesture("");
            gameManager.SetCurProgress(0);
        }

        return true;
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if (progressDisplayed)
            return true;

        return true;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if(gesture == KinectGestures.Gestures.MyRun)
        {
            progressDisplayed = true;
            progressGestureTime = Time.realtimeSinceStartup;

            gameManager.SetCurGesture("MyRun");
            gameManager.SetCurProgress(progress);
        }
    }

    public void UserDetected(long userId, int userIndex)
    {
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.MyRun);

        gameManager.SetIsUserExist(true);
    }

    public void UserLost(long userId, int userIndex)
    {
        gameManager.SetCurGesture("");
        gameManager.SetCurProgress(0);

        gameManager.SetIsUserExist(false);
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
        {
            progressDisplayed = false;

            gameManager.SetCurGesture("");
            gameManager.SetCurProgress(0);

            Debug.Log("Forced progress to end.");
        }
    }
}
