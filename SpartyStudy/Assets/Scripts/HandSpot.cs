using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSpot : MonoBehaviour
{
    public float timeKeepHand = 3.0f; // How long the player has to keep their hand in the box for success
    public float timeBeforeEnd = 6.0f; // How long the player has until this box fails

    public GameObject holdTextObj; // Used to display currTime
    public TextMesh holdText;

    public GameObject warnTextObj; // Used to display currWarnTime
    public TextMesh warnText;

    private float timeStart = 0; // Time that the box first appears
    private int timesEntered = 0; // Number of times hand enters the collider

    private float currWarnTime = 0; // How much time left until fail
    private string timeWarnStr; // currWarnTime as a string

    private float currTime = 0; // How much time left to keep hand in box to take pic
    private string timeStr; // currTime as a string

    private int hands = 0; // Number of hands in this collider
    private bool handIn = false; // Is there a hand in this collider
    private bool firstEntry = false; // Has the first enter happened yet?

    private GameController gameCont; // Used to run the "game" loop
    private SavingData save; // Used for saving data


    private void Update()
    {
        if(currWarnTime <= 0)
        {
            // Ran out of time, failure
            Debug.Log("Hand Failed");
            handIn = false;
            firstEntry = false;
            timesEntered = 0;
            hands = 0;
            currTime = timeKeepHand;
            currWarnTime = timeBeforeEnd;
            warnTextObj.SetActive(false);
            holdTextObj.SetActive(false);
            gameCont.CurrColliderDone(false, timesEntered);
        }

        timeWarnStr = currWarnTime.ToString();
        warnText.text = timeWarnStr;

        if (currTime <= 0)
        {
            // Finished, take pic
            Debug.Log("Hand Complete");
            handIn = false;
            firstEntry = false;
            timesEntered = 0;
            hands = 0;
            currTime = timeKeepHand;
            currWarnTime = timeBeforeEnd;
            holdTextObj.SetActive(false);
            gameCont.CurrColliderDone(true, timesEntered);
        }

        if (handIn)
        {
            // Change the visual timer for keeping hand in the collider
            timeStr = currTime.ToString();
            holdText.text = timeStr;
        }
    }


    public void StartSpot()
    {
        // Excel sheet variable
        gameCont = GameController.instance;
        save = SavingData.instance;
        timeStart = gameCont.timeStarted = Time.realtimeSinceStartup;
        save.AddEvent(gameCont.totalCollide, gameCont.currHandPos, 0, timeStart, "Start");

        handIn = false;
        hands = 0;
        currWarnTime = timeBeforeEnd;
        currTime = timeKeepHand;
        warnTextObj.SetActive(true);
        StartCoroutine(LoseWarnTime());
    }


    private void OnTriggerEnter(Collider other)
    {
        // If the hand enters
        if (other.gameObject.tag == "Hand")
        {
            hands++;
            Debug.Log("Hand Added");

            if (hands == 1)
            {
                ++timesEntered;
                // Excel sheet variable
                // Is this the first time the hand is entering this collider?
                if(firstEntry)
                {
                    // No, not first
                    save.AddEvent(gameCont.totalCollide, gameCont.currHandPos, timesEntered, Time.realtimeSinceStartup, "Enter");
                }
                else
                {
                    // Yes, first
                    firstEntry = true;
                    float startToFirstEnter = Time.realtimeSinceStartup - timeStart;
                    save.AddEvent(gameCont.totalCollide, gameCont.currHandPos, timesEntered, startToFirstEnter, "FirstEnter");
                }

                // First hand entered, start countdown to picture
                handIn = true;
                holdTextObj.SetActive(true);
                warnTextObj.SetActive(false);
                currTime = timeKeepHand;
                currWarnTime = timeBeforeEnd;
                StartCoroutine(LoseTime());
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // If the hand exits
        if (other.gameObject.tag == "Hand")
        {
            hands--;
            Debug.Log("Hand Removed");

            if (hands == 0)
            {
                // Excel sheet variable
                save.AddEvent(gameCont.totalCollide, gameCont.currHandPos, timesEntered, Time.realtimeSinceStartup, "Exit");

                // All hands left, zero out time, stop countdown to picture start countdown to fail again
                currTime = timeKeepHand;
                currWarnTime = timeBeforeEnd;
                handIn = false;
                holdTextObj.SetActive(false);
                warnTextObj.SetActive(true);
                StartCoroutine(LoseWarnTime());
            }
        }
    }


    IEnumerator LoseTime()
    {
        // Countdown for time to keep hand in collider
        while (true)
        {
            yield return new WaitForSeconds(1);
            if(handIn)
            {
                currTime--;
            }
            else
            {
                break;
            }
        }

        yield return null;
    }


    IEnumerator LoseWarnTime()
    {
        // Countdown for fail
        while (true)
        {
            yield return new WaitForSeconds(1);
            if(!handIn)
            {
                currWarnTime--;
            }
            else
            {
                break;
            }
        }

        yield return null;
    }
}
