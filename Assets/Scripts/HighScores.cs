using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScores : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        string s = "";
        for(int i=0; i<10; i++)
        {
            s += (i+1) + " " + PlayerPrefs.GetInt($"highscore{i}") + "\n";
        }
        text.text = s;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetScore(int score)
    {
        for(int i=0; i<10; i++)
        {
            if(score > PlayerPrefs.GetInt($"highscore{i}"))
            {
                for(int n=9; n>i; n--)
                {
                    PlayerPrefs.SetInt($"highscore{n}", PlayerPrefs.GetInt($"highscore{n - 1}"));
                }
                PlayerPrefs.SetInt($"highscore{i}", score);
                return;
            }
        }
    }
}
