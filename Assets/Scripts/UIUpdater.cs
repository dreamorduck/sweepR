using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    public TMP_Text health_ui;
    public TMP_Text level_ui;
    public TMP_Text exp_ui;
    public TMP_Text score_ui;
    public TMP_Text timer_ui;

    public float time = 0f;

    public static MapData data;

    // Start is called before the first frame update
    void Start()
    {
        Settings.SetUpdater(this);
        if(data != null)
            data.FinishReadStruct();
        if (Settings.time > 0)
            time = Settings.time;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Settings.time = time;
        UpdateTime(time);
    }

    public void UpdateHealth(int input)
    {
        health_ui.text = $"Health: {input}";
    }
    public void UpdateLevel(int input)
    {
        level_ui.text = $"Level {input}";
    }
    public void UpdateExp(int input)
    {
        exp_ui.text = $"{input}/{Settings.expCurve[Settings.level-1]}";
    }
    public void UpdateScore(int input)
    {
        score_ui.text = $"Score: {input}";
    }
    public void UpdateTime(float input)
    {
        timer_ui.text = $"{input.ToString("0.00")}";
    }
}
