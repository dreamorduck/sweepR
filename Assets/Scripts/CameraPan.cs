using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraPan : MonoBehaviour
{
    Vector3 previous_pos;
    float sens = 10f;
    float zoom_sens = .1f;
    float flag_zoom_sens = 0.001945525f;
    public Tilemap grid;
    public Canvas flag_canvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(2))
            previous_pos = Input.mousePosition;
        else if(Input.GetMouseButton(2))
        {
            PanCamera(Input.mousePosition);
        }

        //Zoom
        //Camera.main.transform.Translate(new Vector3(0, 0, Input.mouseScrollDelta.y * zoom_sens), Space.World);
        grid.transform.localScale = new Vector3(Mathf.Clamp(grid.transform.localScale.x + (Input.mouseScrollDelta.y * zoom_sens), 0.4f, 2), Mathf.Clamp(grid.transform.localScale.y + (Input.mouseScrollDelta.y * zoom_sens), 0.4f, 2), grid.transform.localScale.z);
        flag_canvas.transform.localScale = new Vector3(Mathf.Clamp(flag_canvas.transform.localScale.x + (Input.mouseScrollDelta.y * flag_zoom_sens), 0.0077821f, 0.0389105f), Mathf.Clamp(flag_canvas.transform.localScale.y + (Input.mouseScrollDelta.y * flag_zoom_sens), 0.0077821f, 0.0389105f), flag_canvas.transform.localScale.z);
    }

    void PanCamera(Vector3 newpos)
    {
        Vector3 offset = Camera.main.ScreenToViewportPoint(previous_pos - newpos);
        Vector3 move = new Vector3(offset.x * sens, offset.y * sens, 0);
        move = Quaternion.Euler(0, 0, 45) * move;
        //Vector3 move = new Vector3(offset.y * sens, offset.x * sens, 0);

        Camera.main.transform.Translate(move, Space.World);

        previous_pos = newpos;
    }
}
