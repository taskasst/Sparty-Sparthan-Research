using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMPro.TMP_InputField textBox; // Text box that the participant number is entered into

    private SavingData save; // Used for saving data
    private int partNum; // The participant number


    private void Start()
    {
        save = SavingData.instance;

        // Excel sheet variable
        save.dateTime = System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
    }


    public void GreenSparty()
    {
        if(GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Sparty";
            save.color = "Green";

            SceneManager.LoadScene("GreenSparty");
        }
    }

    public void PurpleSparty()
    {
        if (GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Sparty";
            save.color = "Purple";

            SceneManager.LoadScene("PurpleSparty");
        }
    }

    public void GreenSparthan()
    {
        if (GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Sparthan";
            save.color = "Green";

            SceneManager.LoadScene("GreenSparthan");
        }
    }

    public void PurpleSparthan()
    {
        if (GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Sparthan";
            save.color = "Purple";

            SceneManager.LoadScene("PurpleSparthan");
        }
    }


    public void GreenEthan()
    {
        if (GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Ethan";
            save.color = "Green";

            SceneManager.LoadScene("GreenEthan");
        }
    }

    public void PurpleEthan()
    {
        if (GoodNumber())
        {
            // Excel sheet variable
            save.spartyType = "Ethan";
            save.color = "Purple";

            SceneManager.LoadScene("PurpleEthan");
        }
    }


    public void RandomLevel()
    {
        // Load a random option
        int level = Random.Range(1, 7); // will give a num 1-4
        if (level == 1)
        {
            GreenSparty();
        }
        else if (level == 2)
        {
            PurpleSparty();
        }
        else if (level == 3)
        {
            GreenSparthan();
        }
        else if (level == 4)
        {
            PurpleSparthan();
        }
        else if (level == 5)
        {
            GreenEthan();
        }
        else if (level == 6)
        {
            PurpleEthan();
        }
    }


    private bool GoodNumber()
    {
        // if participant number not entered, give error
        string inputText = textBox.text;

        if (!string.IsNullOrEmpty(inputText))
        {
            // SavingData set the info
            if (int.TryParse(inputText, out partNum))
            {
                // Excel sheet variable
                save.participantNum = partNum;
                return true;
            }
            else
            {
                // Give error if they didn't enter an integer
                Debug.Log("Participant Number not an int!!");
                return false;
            }
        }
        else
        {
            Debug.Log("Missing Participant Number!!");
            return false;
        }
    }
}
