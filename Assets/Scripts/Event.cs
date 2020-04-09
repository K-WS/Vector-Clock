using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Event : MonoBehaviour
{
    public Sprite arrow;

    public GameObject from = null;    // Values to determine if combining vector clock values from other processes
    private GameObject gameController; // Value to access gameController functions

    private Vector3 realPos;           // Keep track of real position whenever changed to a message
    public List<int> vectorClock;     // The list that holds all vector clock values that are passed.

    public bool marked = false;       // Boolean to prevent another vector clock from being added
    public int process = -1;
    
    private void Start()
    {
        gameController = GameObject.Find("GameController");
        realPos = transform.position;
    }

    /// <summary>
    /// Check if an event is clicked on in message mode.
    /// The first one is simply marked as selected.
    /// The second choice causes a relation between them to be added as an arrow.
    /// </summary>
    private void OnMouseDown()
    {
        //Mark first event
        if (gameController.GetComponent<ProcessCounter>().GetMode() == "message1" && marked == false)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            gameController.GetComponent<ProcessCounter>().SetMode("message2");
            gameController.GetComponent<ProcessCounter>().eventState.text = "Message(2)";

            gameController.GetComponent<ProcessCounter>().message1 = gameObject;
            marked = true;

        }
        //Mark second event, convert to message arrow...
        else if (gameController.GetComponent<ProcessCounter>().GetMode() == "message2" && marked == false) 
        {

            from = gameController.GetComponent<ProcessCounter>().message1;
            

            //Blocking if to make sure you can't put a message on the same process
            if (from.transform.parent != transform.parent)
            {

                gameController.GetComponent<ProcessCounter>().message1 = null;

                GetComponent<SpriteRenderer>().sprite = arrow;

                Vector3 p1 = new Vector3(from.transform.position.x, from.GetComponentInParent<Transform>().position.y);
                Vector3 p2 = new Vector3(transform.position.x, GetComponentInParent<Transform>().position.y);
                transform.position = (p1 + p2) / 2.0f;

                realPos = p2;

                Quaternion rot = Quaternion.LookRotation(p2 - p1);
                rot.x = transform.rotation.x;
                rot.y = transform.rotation.y;
                transform.rotation = rot;
                
                float dist = Vector2.Distance(p1, p2);
                transform.localScale = new Vector3(10 * dist / 128, //arrow x resolution 1280, with 0.1 scale 128
                                                   transform.localScale.y / 2,
                                                   transform.localScale.z);

                //Reposition and scale end collider back to the correct spot
                GetComponent<CircleCollider2D>().offset = new Vector2(6.4f, 0); //Not sure why 6.4 is correct
                GetComponent<CircleCollider2D>().radius = 2.5f/dist;


                gameController.GetComponent<ProcessCounter>().SetMode("message1");
                gameController.GetComponent<ProcessCounter>().eventState.text = "Message(1)";

                marked = true;
            }
        }
    }


    public void InitializeClockList(int count)
    {
        for(int i = 0; i < count; i++)
        {
            vectorClock.Add(0);
        }
    }
    public void WriteMessage()
    {
        string toWrite = "( ";

        foreach (int i in vectorClock)
        {
            toWrite += i + " ";
        }

        toWrite += ")";

        GetComponent<Text>().text = toWrite;
    }


}
