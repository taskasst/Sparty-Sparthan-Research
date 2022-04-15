using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPlacement : MonoBehaviour
{
    public static HandPlacement instance;

    public GameObject upperBack; // The collider for upper back
    public GameObject middleBack; // The collider for middle back
    public GameObject lowerBack; // The collider for lower back
    public GameObject theButt; // The collider for the behind


    private void Awake()
    {
        // Only one instance
        if (!instance)
        {
            instance = GetComponent<HandPlacement>();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        if (!upperBack || !middleBack || !lowerBack || !theButt)
        {
            Debug.LogError("Missing hand colliders.");
        }
    }


    public void EnableHandCollider(int position)
    {
        if (position == 1)
        {
            // Upper back
            upperBack.SetActive(true);
            upperBack.GetComponent<HandSpot>().StartSpot();
        }
        else if (position == 2)
        {
            // Middle back
            middleBack.SetActive(true);
            middleBack.GetComponent<HandSpot>().StartSpot();
        }
        else if (position == 3)
        {
            // Butt
            lowerBack.SetActive(true);
            lowerBack.GetComponent<HandSpot>().StartSpot();
        }
        else if (position == 4)
        {
            // Butt
            theButt.SetActive(true);
            theButt.GetComponent<HandSpot>().StartSpot();
        }
    }


    public void RemoveHandColliders()
    {
        // Disable all of the colliders
        upperBack.SetActive(false);
        middleBack.SetActive(false);
        lowerBack.SetActive(false);
        theButt.SetActive(false);
    }
}
