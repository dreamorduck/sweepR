using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

    public class Settings
    {
    //Options
    public static int musicVolume;
    public static int sfxVolume;

    //Ingame stats
    private static int _hp;
    public static int attack;
    private static int _exp;
    public static List<int> expCurve = new List<int>();
    public static int level;
    private static int _score;
    public static float time;

    private static UIUpdater uiUpdater;

    public static void SetUpdater(UIUpdater input)
    {
        uiUpdater = input;
    }

    public static int health
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            uiUpdater.UpdateHealth(_hp);
            if (_hp <= 0)
            {
                EscMenu.isIngame = false;
                SceneManager.LoadScene("lose");
            }
        }
    }

    public static int exp
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
            if(_exp >= expCurve[level-1])
            {
                _exp -= expCurve[level - 1];
                level++;
            }
            if (level == expCurve.Count + 1)
            {
                //SceneManager.LoadScene("win");
                score += (int)Math.Floor(10000 - time);
                score += _hp * 2000;
                HighScores.SetScore(score);
                EscMenu.isIngame = false;
                SceneManager.LoadScene("win");
            }
            else
            {
                uiUpdater.UpdateExp(_exp);
                uiUpdater.UpdateLevel(level);
            }
        }
    }

    public static int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            uiUpdater.UpdateScore(_score);
        }
    }

    private void WinGame()
    {
        //score += (int)Math.Floor(time * 1000);
        score += (int)Math.Floor(10000 - time);
        score += _hp * 2000;
        HighScores.SetScore(score);
        SceneManager.LoadScene("win");
    }
    }
