using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHuman : MonoBehaviour
{
    public static SpawnHuman instance;

    public GameObject[] humans; // List of all (10) models in order
    public GameObject[] picTexts; // List of all the "can we take a pic" texts

    private GameObject currModel; // The current model visible
    private int currIndex = 0; // The current model index (0-9)
    private int humansLength; // Number of models we have (9, 10 subtract 1 for coding purposes)

    private Vector3 pos; // The position to place the model
    private Quaternion rot; // The rotation to give the model
    private GameController gameCont; // Used to run the "game" loop
    private HandPlacement handPlace; // Used to set position of colliders

    private GameObject upperBack; // The collider for upper back
    private GameObject middleBack; // The collider for middle back
    private GameObject lowerBack; // The collider for lower back
    private GameObject theButt; // The collider for the behind


    private void Awake()
    {
        // Only one instance
        if (!instance)
        {
            instance = GetComponent<SpawnHuman>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        gameCont = GameController.instance;
        handPlace = HandPlacement.instance;

        upperBack = handPlace.upperBack;
        middleBack = handPlace.middleBack;
        lowerBack = handPlace.lowerBack;
        theButt = handPlace.theButt;

        Instantialize();
    }


    private void Instantialize()
    {
        // Get this object's info
        pos = transform.position;
        rot = transform.rotation;

        if (humans.Length == 0)
        {
            Debug.LogError("There are no models in ModelPlacement!");
        }
        else
        {
            // Get how many human models we have
            humansLength = humans.Length - 1;
            currIndex = 0;
        }
    }


    public void InstantiateModel()
    {
        // Instantiate the next human model at position pos with rotation rot
        Vector3 position = pos + humans[currIndex].transform.position;
        Quaternion rotation = new Quaternion(0, rot.y + humans[currIndex].transform.rotation.y, 0, 1);
        currModel = Instantiate(humans[currIndex], position, rotation);
        gameCont.currPerson = currIndex;
        picTexts[currIndex].SetActive(true);

        // Set positions of the box colliders depending on model
        ColliderLocation collLoc = currModel.GetComponent<ColliderLocation>();
        upperBack.transform.position = collLoc.upperSpot.transform.position;
        middleBack.transform.position = collLoc.middleSpot.transform.position;
        lowerBack.transform.position = collLoc.lowerSpot.transform.position;
        theButt.transform.position = collLoc.buttSpot.transform.position;
    }


    public void DeleteModel()
    {
        // Delete the current human model and increment to allow the next one to be made
        Destroy(currModel);
        picTexts[currIndex].SetActive(false);
        currIndex++;

        if (currIndex > humansLength)
        {
            // Went through all the humans, end game
            currIndex = 0;
            Debug.Log("Game done");
            SavingData.instance.WriteData();
            Application.Quit();
        }
    }
}
