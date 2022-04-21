using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public static List<Achievement> achievements;
    GameObject obj;

    public void Start()
    {
        obj = (GameObject)Resources.Load("prefabs/Achievement");
        //For testing purposes.
        InstantiateAchievements();
    }

    public static void InstantiateAchievements()
    {
        achievements = new List<Achievement>();
        for (int i = 0; i < 25; i++)
        {
            Achievement a = new Achievement();
            a.id = i;
            a.title = $"Test achievement {i}";
            a.description = $"This is a test {i}{i}{i}.";
            if (i % 2 == 0)
                a.img = "Tile/Images/clicked";
            else
                a.img = "Tile/Images/unclicked";
            achievements.Add(a);
        }
    }

    public static void CompleteAchievement(int id)
    {
        for(int i=0; i<achievements.Count; i++)
        {
            if(id == achievements[i].id)
            {
                achievements[i].isComplete = true;
            }
        }
    }

    public static void CompleteAchievement(string title)
    {
        for(int i=0; i<achievements.Count; i++)
        {
            if (title == achievements[i].title)
                CompleteAchievement(achievements[i].id);
        }
    }

    public static Achievement Find(int id)
    {
        for(int i=0; i<achievements.Count;i ++)
        {
            if (id == achievements[i].id)
                return achievements[i];
        }
        Debug.Log($"Failed to find achievement {id}.");
        return achievements[0];
    }
}
