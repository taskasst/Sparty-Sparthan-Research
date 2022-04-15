using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandSpot : MonoBehaviour
{
    public int timeKeepHand = 5; // How long they have to keep their hand in the collider

    public GameObject holdTextObj; // Used to display currTime
    public TextMesh holdText;
    
    private int currTime = 0; // Time left to keep hand in collider
    private string timeStr; // String version of currTime

    private int hands = 0; // Number of hands in the collider
    private bool handIn = false; // Is there a hand in the collider?

    private Tutorial tutorial; // Used to run the tutorial


    private void Start()
    {
        tutorial = Tutorial.instance;
        currTime = timeKeepHand;
    }


    private void Update()
    {
        if(currTime <= 0)
        {
            // Finished
            currTime = timeKeepHand;
            holdTextObj.SetActive(false);
            tutorial.CurrColliderDone();
        }

        if(handIn)
        {
            // Change the visual timer
            timeStr = currTime.ToString();
            holdText.text = timeStr;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // If the hand enters
        if (other.gameObject.tag == "Hand")
        {
            hands++;

            if(hands == 1)
            {
                // Start countdown
                handIn = true;
                holdTextObj.SetActive(true);
                currTime = timeKeepHand;
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

            if (hands == 0)
            {
                // All hands left, zero out time
                currTime = timeKeepHand;
                handIn = false;
                holdTextObj.SetActive(false);
            }
        }
    }


    IEnumerator LoseTime()
    {
        // Used to count down time to keep hand in collider
        while (true)
        {
            yield return new WaitForSeconds(1);
            if(handIn)
            {
                currTime--;
            }
            else
            {
                currTime = timeKeepHand;
                break;
            }
        }

        yield return null;
    }
}
