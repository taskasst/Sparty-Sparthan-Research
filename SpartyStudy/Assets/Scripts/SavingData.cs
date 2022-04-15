using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingData : MonoBehaviour
{
    public static SavingData instance = null;

    [System.Serializable]
    public struct EventStruct
    {
        public int colliderNum; // 1-20
        public int colliderType; // 1-4
        public int numEntries; // how many times the player put their hand in the collider
        public float timeEvent; // time since start (or time difference between events)
        public string eventType; // Enter, picture, etc
    }

    public int participantNum; // An integer
    public string dateTime; // The date and time the study was started
    public string spartyType; // "Sparty" or "Sparthan"
    public string color; // "Purple" or "Green"
    public List<EventStruct> eventsList; // Time and event type

    private float timeUpper;
    private float timeMiddle;
    private float timeLower;
    private float timeBehind;


    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)

            // If not, set instance to this
            instance = this;

        // If instance already exists and it's not this:
        else if (instance != this)

            // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of.
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        timeUpper = 0;
        timeMiddle = 0;
        timeLower = 0;
        timeBehind = 0;
    }


    // Called at end
    // Assumes csv file already exists
    public void WriteData()
    {
        // Create stream writer that will write to file without overwriting 
        System.IO.StreamWriter sw = System.IO.File.AppendText("mascotdata.csv");

        foreach (EventStruct eve in eventsList)
        {
            // Prepare string to be inserted into csv file
            // Participant Number, Date and Time, Sparty Type, Color, 
            // Collider Number, Model Number, 
            // Collider Type, Times Entered, Event Time, Event Type
            string line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                participantNum.ToString(), dateTime, spartyType, color, 
                eve.colliderNum.ToString(), ((eve.colliderNum + 1) / 2).ToString(), 
                eve.colliderType.ToString(), eve.numEntries.ToString(), eve.timeEvent.ToString(), eve.eventType);

            // Appends Line to csv file
            sw.WriteLine(line);

            // Sum of time elapsed per collider type
            if(eve.eventType == "TimeElapsed")
            {
                if(eve.colliderType == 1)
                {
                    timeUpper += eve.timeEvent;
                }
                else if (eve.colliderType == 2)
                {
                    timeMiddle += eve.timeEvent;
                }
                else if (eve.colliderType == 3)
                {
                    timeLower += eve.timeEvent;
                }
                else if (eve.colliderType == 4)
                {
                    timeBehind += eve.timeEvent;
                }
            }
        }

        string elapsedLine = string.Format("{0},{1},{2},{3}\n", timeUpper, timeMiddle, timeLower, timeBehind);

        sw.WriteLine(elapsedLine);

        sw.Close();
    }


    public void AddEvent(int cNum, int cType, int numEnt, float t, string e)
    {
        EventStruct ev = new EventStruct();
        ev.colliderNum = cNum;
        ev.colliderType = cType;
        ev.numEntries = numEnt;
        ev.timeEvent = t;
        ev.eventType = e;

        eventsList.Add(ev);
    }
}
