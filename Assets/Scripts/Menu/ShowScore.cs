using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    public TMP_Text text;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = $"Score: {Settings.score}";
    }
}
