using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    public List<GameObject> colliders; // List of test colliders
    public GameObject tutText; // Text to display during tutorial

    private GameController gameCont; // Used to start the actual study after tutorial is done

    private int numColliders = 0; // Total number of colliders
    private int currCollider = 0; // Collider we're on


    private void Awake()
    {
        // Only one instance
        if (!instance)
        {
            instance = GetComponent<Tutorial>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        gameCont = GameController.instance;

        // Get length of the tutorial colliders
        numColliders = colliders.Count;
    }


    public void StartTutorial()
    {
        // Start the tutorial
        tutText.SetActive(true);
        ActivateCollider();
    }


    private void ActivateCollider()
    {
        if (currCollider >= numColliders)
        {
            // Done with tutorial
            Debug.Log("Tutorial Done.");
            EndTut();
        }
        else
        {
            // Activate next tutorial collider
            colliders[currCollider].SetActive(true);
        }
    }


    public void CurrColliderDone()
    {
        // This collider's time has completed, remove it and activate next one
        colliders[currCollider].SetActive(false);
        currCollider++;
        ActivateCollider();
    }


    public void EndTut()
    {
        // Tutorial complete, move on to actual experience
        tutText.SetActive(false);
        gameCont.StartGame();
    }
}
