using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public Text ui_display;
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            //ui_display.text = hit.collider.gameObject.name;
            ui_display.text = hit.collider.gameObject.GetComponent<Text>().text;
        }
        else
        {
            ui_display.text = "Hover over processes/events/message after pressing Start to display results here.";
        }

        //Give ability to close with ESC
        if (Input.GetKeyDown("escape"))
            Application.Quit();
    }

}