using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessCounter : MonoBehaviour
{
    public Button less;
    public Button more;
    public Button eventB;
    public Button messageB;
    public Button start;
    public Button reset;

    public GameObject sideLine;
    public GameObject processPrefab;
    public GameObject processKeeper;

    public Text processCount;
    private int procCount = 0;

    public Text eventState;

    private string mode = "event"; //CAN BE EVENT, MESSAGE1, MESSAGE2, EVENTS AND BUTTONS CONTROL

    public GameObject message1 = null;

    /// <summary>
    /// More processes button, adds a process. Limit 20.
    /// </summary>
    public void MorePressed() 
    {
        procCount += 1;
        if (procCount > 20)
            procCount = 20;

        else if (procCount == 1)
        {
            sideLine.SetActive(true);

            GameObject go = Instantiate(processPrefab, processKeeper.transform);
            go.transform.position = new Vector3(0, 4.2f - (procCount-1) * 0.4f, 0);

            processKeeper.GetComponent<ProcessKeeper>().addToList(go);
        }

        else 
        {
            sideLine.transform.localScale += new Vector3(0, 0.4f);
            sideLine.transform.localPosition -= new Vector3(0, 0.2f);

            GameObject go = Instantiate(processPrefab, processKeeper.transform);
            go.transform.position = new Vector3(0, 4.2f - (procCount-1)*0.4f, 0);

            processKeeper.GetComponent<ProcessKeeper>().addToList(go);
        }

        processCount.text = procCount.ToString();
    }

    /// <summary>
    /// Less processes button, removes a process. Limit None.
    /// </summary>
    public void LessPressed()
    {
        procCount -= 1;
        if (procCount < 0)
            procCount = 0;

        else if (procCount == 0)
        {
            sideLine.SetActive(false);
            processKeeper.GetComponent<ProcessKeeper>().removeFromList();
        }
            

        else
        {
            sideLine.transform.localScale -= new Vector3(0, 0.4f);
            sideLine.transform.localPosition += new Vector3(0, 0.2f);

            processKeeper.GetComponent<ProcessKeeper>().removeFromList();

        }

        processCount.text = procCount.ToString();
    }

    /// <summary>
    /// Event button, allows adding of events on processes
    /// </summary>
    public void EventPressed()
    {
        eventState.text = "Event";
        mode = "event";

        if(message1 != null)
        {
            message1.GetComponent<SpriteRenderer>().color = Color.black;
            message1.GetComponent<Event>().marked = false;
            message1 = null;
        }

        processKeeper.GetComponent<ProcessKeeper>().ProcessState(true);

        eventB.gameObject.SetActive(false);
        messageB.gameObject.SetActive(true);
    }

    /// <summary>
    /// Message button, changes 2 events on different processes into a message
    /// </summary>
    public void MessagePressed()
    {
        eventState.text = "Message(1)";
        mode = "message1";
        processKeeper.GetComponent<ProcessKeeper>().ProcessState(false);

        eventB.gameObject.SetActive(true);
        messageB.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start button, calculates Vector Clock times for each event and message
    /// </summary>
    public void StartPressed()
    {
        eventState.text = "End";
        mode = "end";
        processKeeper.GetComponent<ProcessKeeper>().Calculate();
        processKeeper.GetComponent<ProcessKeeper>().ProcessState(false);


        if (message1 != null)
        {
            message1.GetComponent<SpriteRenderer>().color = Color.black;
            message1.GetComponent<Event>().marked = false;
            message1 = null;
        }

        eventB.gameObject.SetActive(false);
        messageB.gameObject.SetActive(false);
        start.gameObject.SetActive(false);


    }

    /// <summary>
    /// Reset button, removes all processes and reenables addition of events/messages
    /// </summary>
    public void ResetPressed()
    {
        while (procCount > 0)
        {
            LessPressed();
        }
        eventState.text = "Event";
        mode = "event";

        eventB.gameObject.SetActive(false);
        messageB.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
    }

    public string GetMode() { return mode; }
    public void SetMode(string ns) { mode = ns; }
}
