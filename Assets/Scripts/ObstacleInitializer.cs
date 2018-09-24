using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInitializer : MonoBehaviour {

    public string accessoriesPrefabPath;
    public List<Transform> accessoriesTransList = new List<Transform>();
    private List<GameObject> currentAccessories = new List<GameObject>();

    public string fixedNPCPrefabPath;
    public List<Transform> fixedNPCTransList = new List<Transform>();
    private List<GameObject> currentFixedNPC = new List<GameObject>();

    public string movingNPCPrefabPath;
    public List<Transform> movingNPCTransList = new List<Transform>();
    private List<GameObject> currentMovingNPC = new List<GameObject>();

    public bool isAudioStopped;

    public int sky;

	// Use this for initialization
	void Start () {
        // Initialize accessories 
        GameObject[] accessoriesArray = Resources.LoadAll<GameObject>(accessoriesPrefabPath);

        foreach(Transform accTrans in accessoriesTransList)
        {
            if(Random.value > 0.4)  // Randomly decide whether an accessory appears. 
            {
                GameObject newAcc = accessoriesArray[Random.Range(0, accessoriesArray.Length)];
                newAcc = Instantiate(newAcc, accTrans);

                currentAccessories.Add(newAcc);
            }
        }

        // When the weather is bad, there is no NPC.
        if(sky == (int)RunController.MySky.Rain
            || sky == (int)RunController.MySky.Snow
            || sky == (int)RunController.MySky.Night)
        {
            return;
        }

        // Keep audio playing.
        isAudioStopped = false;

        // Initialize fixed NPC
        GameObject[] fixedNPCArray = Resources.LoadAll<GameObject>(fixedNPCPrefabPath);

        foreach(Transform fixTrans in fixedNPCTransList)
        {
            if(Random.value > 0.4)
            {
                GameObject newFix = fixedNPCArray[Random.Range(0, fixedNPCArray.Length)];
                newFix = Instantiate(newFix, fixTrans);

                currentFixedNPC.Add(newFix);
                StartCoroutine(PlayNPCSounds(newFix));  // Control the audio
            }
        }

        // Initialize moving NPC
        GameObject[] movingNPCArray = Resources.LoadAll<GameObject>(movingNPCPrefabPath);

        foreach(Transform movTrans in movingNPCTransList)
        {
            if(Random.value > 0.4)
            {
                GameObject newMov = movingNPCArray[Random.Range(0, movingNPCArray.Length)];
                newMov = Instantiate(newMov, movTrans);
                if (Random.value > 0.5)
                {
                    newMov.transform.Rotate(new Vector3(0, 180, 0));
                }

                currentMovingNPC.Add(newMov);
                StartCoroutine(PlayNPCSounds(newMov));  // Control the audio
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        foreach(GameObject movNPC in currentMovingNPC)
        {
            movNPC.transform.position += new Vector3(
                0,
                0,
                movNPC.transform.forward.z) * Time.deltaTime;

            if(movNPC.transform.position.z > this.transform.position.z +37 
                || movNPC.transform.position.z < this.transform.position.z - 37)
            {
                movNPC.transform.Rotate(new Vector3(0, 180f, 0));
                if(movNPC.transform.position.z > this.transform.position.z + 37)
                {
                    movNPC.transform.position = new Vector3(
                        movNPC.transform.position.x,
                        movNPC.transform.position.y,
                        this.transform.position.z + 37);
                }
                else
                {
                    movNPC.transform.position = new Vector3(
                        movNPC.transform.position.x,
                        movNPC.transform.position.y,
                        this.transform.position.z - 37);
                }
            }
        }
	}

    // OnDestroy is called when being destroyed
    private void OnDestroy()
    {
        // Destroy obstacles
        foreach(GameObject gameObject in currentAccessories)
        {
            Destroy(gameObject,3);
        }

        // Audio stop playing. 
        isAudioStopped = true;

    }

    // Play NPC Sounds
    IEnumerator PlayNPCSounds(GameObject NPC)
    {
        yield return new WaitForSeconds(Random.Range(4f, 50f));

        if(isAudioStopped == true)
        {
            yield break;
        }

        AudioSource audioSource = NPC.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip,2);

        yield return new WaitForSeconds(audioSource.clip.length);
        StartCoroutine(PlayNPCSounds(NPC));
    }
}
