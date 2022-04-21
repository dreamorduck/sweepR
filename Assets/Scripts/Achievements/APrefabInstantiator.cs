using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APrefabInstantiator : MonoBehaviour
{
    public int id;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetImg()
    {
        string path = Achievements.Find(id).img;
        img.sprite = Resources.Load<Sprite>(path);
    }
}
