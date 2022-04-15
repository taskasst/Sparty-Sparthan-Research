using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHands : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public Vector3 leftRotV;
    public Vector3 rightRotV;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            leftHand.transform.eulerAngles = leftRotV ;
            rightHand.transform.eulerAngles = rightRotV ;
        }
    }
}
