using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GenerationButtons : MonoBehaviour
{
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    //Color gray = new Color(76, 75, 76);
    Color gray = new Color(0.4627451f, 0.4588236f, 0.4627451f);
    //Color gray = Color.black;

    private void Start()
    {
        //Set initial values if it's the first time game is run.
        if (PlayerPrefs.GetInt("width") < 10)
            InitialValues.InitializePrefs();

        widthInput.text = PlayerPrefs.GetInt("width").ToString();
        heightInput.text = PlayerPrefs.GetInt("height").ToString();
        string diff = PlayerPrefs.GetString("difficulty");
        if (diff == "medium")
            SetMedium();
        else if (diff == "hard")
            SetHard();
        else if (diff == "custom")
            SetCustom();
        else
            SetEasy();
    }

    public void UpdateInt(string pref)
    {
        TMP_InputField field;
        switch(pref)
        {
            default:
            case "width":
                field = widthInput;
                break;
            case "height":
                field = heightInput;
                break;
        }
        int i = Int32.Parse(field.text);
        if (i > 100)
        {
            i = 100;
            field.text = i.ToString();
        }
        if (i < 10)
        {
            i = 10;
            field.text = i.ToString();
        }
        PlayerPrefs.SetInt(pref, i);
    }

    public void SetEasy()
    {
        widthInput.text = "10";
        heightInput.text = "10";
        widthInput.enabled = false;
        widthInput.image.color = gray;
        heightInput.enabled = false;
        heightInput.image.color = gray;
        PlayerPrefs.SetInt("width", 10);
        PlayerPrefs.SetInt("height", 10);
        PlayerPrefs.SetString("difficulty", "easy");
    }

    public void SetMedium()
    {
        widthInput.text = "15";
        heightInput.text = "10";
        widthInput.enabled = false;
        widthInput.image.color = gray;
        heightInput.enabled = false;
        heightInput.image.color = gray;
        PlayerPrefs.SetInt("width", 15);
        PlayerPrefs.SetInt("height", 10);
        PlayerPrefs.SetString("difficulty", "medium");
    }

    public void SetHard()
    {
        widthInput.text = "15";
        heightInput.text = "15";
        widthInput.enabled = false;
        widthInput.image.color = gray;
        heightInput.enabled = false;
        heightInput.image.color = gray;
        PlayerPrefs.SetInt("width", 15);
        PlayerPrefs.SetInt("height", 15);
        PlayerPrefs.SetString("difficulty", "hard");
    }

    public void SetCustom()
    {
        widthInput.enabled = true;
        widthInput.image.color = Color.white;
        heightInput.enabled = true;
        heightInput.image.color = Color.white;
        PlayerPrefs.SetString("difficulty", "custom");
    }
}
