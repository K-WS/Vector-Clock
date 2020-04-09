using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Process : MonoBehaviour
{

    public List<GameObject> events = new List<GameObject>();
    public GameObject processKeeper;
    //private List<GameObject> messageOrder = new List<GameObject>();
    //private List<int> messageIndices = new List<int>();
    public GameObject eventPrefab;
    public int process = -1;



    private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.Find("GameController");
        processKeeper = GameObject.Find("ProcessKeeper");
    }
    /// <summary>
    /// Instantiate events on the selected process when clicked on in event mode
    /// </summary>
    private void OnMouseDown()
    {
        if (gameController.GetComponent<ProcessCounter>().GetMode() == "event")
        {
            float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float y = transform.position.y;

            //Give event a from/to to know where to transfer, place in array based on position
            GameObject go = Instantiate(eventPrefab, transform);
            //Also make sure to let them know which process number they belong to
            go.GetComponent<Event>().process = process;
            go.transform.position = new Vector3(x, y, 0);

            if (events.Count == 0)
                events.Add(go);
            else
            {
                int addingPos = 0;

                for (int i = 0; i < events.Count; i++)
                {
                    if (x > events[i].transform.position.x)
                        addingPos += 1;
                    else
                        break;
                }

                events.Insert(addingPos, go);
            }
        }
    }


    /// <summary>
    /// Calculate the value each event is supposed to have;
    /// After an event is done, pass its result to the second event and do it again
    /// Also, collect all messages locations for the next function
    /// </summary>
    /// <param name="process"></param>
    public void Calculate(int processes)
    {
        int n = events.Count;

        //Grab Event
        for (int i = 0; i < n; i++)
        {
            GameObject go = events[i];
            //Initial assingment
            go.GetComponent<Event>().InitializeClockList(processes);
            go.GetComponent<Event>().vectorClock[process] = i + 1;
            go.GetComponent<Event>().WriteMessage();

            //Check if message, if so, give to ProcessKeeper
            if (go.GetComponent<Event>().from != null)
            {
                List<GameObject> messageOrder = processKeeper.GetComponent<ProcessKeeper>().messageOrder;
                List<int> messageIndices = processKeeper.GetComponent<ProcessKeeper>().messageIndices;

                if (messageOrder.Count == 0)
                {
                    messageOrder.Add(go);
                    messageIndices.Add(i);
                }
                    

                else
                {
                    int addingPos = 0;
                    for (int j = 0; j < messageOrder.Count; j++)
                    {
                        if (go.GetComponent<Event>().from.transform.position.x
                            >
                            messageOrder[j].GetComponent<Event>().from.transform.position.x)
                            addingPos += 1;
                        else
                            break;
                    }
                    messageOrder.Insert(addingPos, go);
                    messageIndices.Insert(addingPos, i);
                }
            }
        }


    }

    /// <summary>
    /// Extension of Calculate; after every process has their values, check and grab message "from" values as well
    /// IF ITS UPDATED SOMEWHERE, IT HAS TO PASS IT ON AGAIN
    /// </summary>
    public void Message(int start)
    {
        //Get the starting event and create a dictionary to store all the "from" message values
        GameObject startEvent = events[start];
        Dictionary<int, int> newVals = new Dictionary<int, int>();


        //Get the vector clock list from the "from" message
        List<int> passClock = startEvent.GetComponent<Event>().from.GetComponent<Event>().vectorClock;

        //Go through the "from" message and store all values that aren't the selected process index itself
        for (int i = 0; i < passClock.Count; i++)
        {
            if (i != process)
            {
                if (!newVals.ContainsKey(i))
                    newVals.Add(i, passClock[i]);
                else
                    newVals[i] = passClock[i];
            }
        }

        //Go through each event from starting position to end, giving them the new key values
        for(int i = start; i < events.Count; i++)
        {
            GameObject go = events[i];

            foreach (KeyValuePair<int, int> entry in newVals)
            {
                go.GetComponent<Event>().vectorClock[entry.Key] = Mathf.Max(entry.Value, go.GetComponent<Event>().vectorClock[entry.Key]);
                
            }

            go.GetComponent<Event>().WriteMessage();
        }

        


    }
    

    
}
