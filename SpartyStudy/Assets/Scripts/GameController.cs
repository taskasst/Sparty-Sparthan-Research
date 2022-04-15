using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Vector2Int[] pattern; // Order of collider placement for each model
    public GameObject picLight; // The "camera flash"

    public float timeTransition = 2.0f; // Time to wait to show collider
    public float timeFade = 2.0f; // Time it takes to fade to black or fade back

    public int totalCollide = 1; // Current collider number (1-20)
    public int currHandPos = 1; // Position of the hand (upper/middle/lower/behind 1-4)
    public int currPerson = 0; // The index of the current model (0-9)

    public float timeStarted = 0.0f; // Time of event Start

    public bool skipTutorial = false; // Used for testing

    private bool started = false; // Has it started/did they press P
    
    private Tutorial tutorial; // Used to run the tutorial
    private SpawnHuman spawn; // Used to spawn the models
    private HandPlacement place; // Used to change the colliders

    private int currCollider = 1; // Used to switch to next model (1-2) as there are two colliders per model


    private void Awake()
    {
        // Only one instance
        if (!instance)
        {
            instance = GetComponent<GameController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        spawn = SpawnHuman.instance;
        place = HandPlacement.instance;
        tutorial = Tutorial.instance;
    }


    private void Update()
    {
        // Start the game
        if (Input.GetKeyDown(KeyCode.P) && !started)
        {
            started = true;
            if (skipTutorial)
            {
                StartGame();
            }
            else
            {
                tutorial.StartTutorial();
            }
        }
    }


    public void StartGame()
    {
        // Instantiate the first model
        StartCoroutine(StartFade());
    }


    private void ActivateCollider()
    {
        if (currCollider > 2)
        {
            // Done with this model
            currCollider = 1;
            StartCoroutine(FadeToBlack());
        }
        else
        {
            // Activate next collider after waiting a little amount of time
            StartCoroutine(WaitCollider());
        }
    }


    public void CurrColliderDone(bool success, int numEntered)
    {
        // This collider's time has completed, remove it and activate next one
        // Excel sheet variable
        if(success)
        {
            float timeEnd = Time.realtimeSinceStartup;
            SavingData.instance.AddEvent(totalCollide, currHandPos, numEntered, timeEnd, "Picture");
            // Also add one for the change in time from start
            float timeDiff = timeEnd - timeStarted;
            SavingData.instance.AddEvent(totalCollide, currHandPos, numEntered, timeDiff, "TimeElapsed");
        }
        else
        {
            SavingData.instance.AddEvent(totalCollide, currHandPos, numEntered, Time.realtimeSinceStartup, "Fail");
        }

        place.RemoveHandColliders();
        Debug.Log("Removing collider");
        totalCollide++;

        // Camera flash to "take" picture
        if(success)
        {
            StartCoroutine(TakePic());
        }
        else
        {
            currCollider++;

            ActivateCollider();
        }
    }


    IEnumerator StartFade()
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, timeFade);

        yield return new WaitForSeconds(timeFade);
        
        spawn.InstantiateModel();
        StartCoroutine(FadeBack());

        yield return null;
    }


    IEnumerator FadeToBlack()
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, timeFade);
        
        yield return new WaitForSeconds(timeFade);

        spawn.DeleteModel();
        spawn.InstantiateModel();
        StartCoroutine(FadeBack());

        yield return null;
    }


    IEnumerator FadeBack()
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, timeFade);

        yield return new WaitForSeconds(timeFade);

        ActivateCollider();
        yield return null;
    }


    IEnumerator WaitCollider()
    {
        // Wait a couple seconds to show the collider
        yield return new WaitForSeconds(timeTransition);

        // Turn on the collider based on the pattern we made
        if (currCollider == 1)
        {
            currHandPos = pattern[currPerson].x;
            place.EnableHandCollider(pattern[currPerson].x);
        }
        else if (currCollider == 2)
        {
            currHandPos = pattern[currPerson].y;
            place.EnableHandCollider(pattern[currPerson].y);
        }

        Debug.Log("Starting collider");
    }


    IEnumerator TakePic()
    {
        // Play camera sound
        picLight.GetComponent<AudioSource>().Play();
        // Enable light
        picLight.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // Disable light
        picLight.SetActive(false);

        currCollider++;

        ActivateCollider();
    }
}
