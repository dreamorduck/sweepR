using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuToggle : MonoBehaviour
{
    public Canvas canvas;
    public static bool isVisible;

    // Start is called before the first frame update
    void Start()
    {
        isVisible = false;
        canvas.gameObject.SetActive(isVisible);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isVisible = !isVisible;
            canvas.gameObject.SetActive(isVisible);
        }
    }

    //This is here for the main menu's quit button
    public void QuitGame()
    {
        Application.Quit();
    }
}
