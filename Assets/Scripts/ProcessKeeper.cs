using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessKeeper : MonoBehaviour
{
    public List<GameObject> processes = new List<GameObject>();
    public List<GameObject> messageOrder = new List<GameObject>();
    public List<int> messageIndices = new List<int>();

    /// <summary>
    /// Add to the main list of processes a new process
    /// </summary>
    /// <param name="go"></param>
    public void addToList(GameObject go)
    {
        processes.Add(go);
        go.GetComponent<Process>().process = processes.Count-1;
        go.GetComponentInChildren<TextMesh>().text = "P" + processes.Count;
     }

    /// <summary>
    /// Remove the last process from the list of processes and destroy it
    /// </summary>
    public void removeFromList()
    {
        GameObject go = processes[processes.Count - 1];
        processes.Remove(go);
        Destroy(go);
    }

    /// <summary>
    /// For each process, mark the starting spot text, then call each process's list of events
    /// Then the process individually gets each event and passes the necessary data.
    /// </summary>
    public void Calculate()
    {
        int n = processes.Count;

        //First for-loop, assign regular clock values to everyone
        for(int arg = 0; arg < n; arg++)
        {
            GameObject go = processes[arg];

            //Write starting text
            Text text = go.transform.Find("ProcText").GetComponent<Text>();
            string toWrite = "( ";
            for (int i = 0; i < n; i++)
            {
                toWrite += "0 ";
            }
            toWrite += ")";
            text.text = toWrite;


            //Call for each process to manually assign text for their events
            go.GetComponent<Process>().Calculate(n);

        }


        //Second loop, go through all of them again to find messages

        for (int arg = 0; arg < messageOrder.Count; arg++)
        {
            GameObject go = messageOrder[arg];
            Event evnt = go.GetComponent<Event>();
            processes[evnt.process].GetComponent<Process>().Message(messageIndices[arg]);
        }

    }

    /// <summary>
    /// Turns the hitboxes for the processes on or off, to ignore a bug where some events
    /// aren't clickable after being created
    /// </summary>
    /// <param name="state"></param>
    public void ProcessState(bool state)
    {
        foreach(GameObject go in processes)
            go.GetComponent<BoxCollider2D>().enabled = state;
    }
}
