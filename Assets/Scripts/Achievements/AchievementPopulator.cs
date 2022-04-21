using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopulator : MonoBehaviour
{
    public RectTransform area;
    GameObject tile;

    int x;
    int y;

    // Start is called before the first frame update
    void Start()
    {
        tile = (GameObject)Resources.Load("prefabs/Achievement");
        x = 0;
        y = 0;

        if (Achievements.achievements == null)
            Achievements.InstantiateAchievements();

        foreach(Achievement a in Achievements.achievements)
        {
            //GameObject.Instantiate<GameObject>(tile, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), area);
            GameObject obj = GameObject.Instantiate<GameObject>(tile, area);
            //GameObject.Instantiate<GameObject>(tile, area);
            //tile.transform.SetParent(area);
            obj.transform.localPosition = new Vector3(x+50, y-50, 0);
            obj.GetComponentInChildren<APrefabInstantiator>().id = a.id;
            obj.GetComponentInChildren<APrefabInstantiator>().SetImg();
            x += 100;
            if(x > 1000)
            {
                x = 0;
                y -= 100;
            }
        }
    }
}
