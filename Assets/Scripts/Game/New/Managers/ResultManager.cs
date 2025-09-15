using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public Text score;
    public Text combo;
    public Text panelScore;
    public Text panelMaxCombo;
    public Text panelPerfect;
    public Text panelGreat;
    public Text panelBad;
    public Text panelMiss;
    public Text panelLevel;

    public int totalScore;
    public int curCombo;
    public int maxCombo;
    public int perfect;
    public int great;
    public int bad;
    public int miss;

    private void Awake()
    {
        init();
    }

    private void init()
    {
        curCombo = 0;
        maxCombo = 0;
        totalScore = 0;
        perfect = 0;
        great = 0;
        bad = 0;
        miss = 0;
    }

    public void ScoreUp(int scores)
    {
        totalScore += scores + (int)(curCombo / (float)GameInfoSingleton.instance.totalCombo * GameInfoSingleton.instance.extraScore);
        if (totalScore > 1000000)
            totalScore = 1000000;
        if (curCombo == GameInfoSingleton.instance.totalCombo && great == 0 && bad == 0 && miss == 0) //如果是按下最后一个音符，并且没有great,bad,miss记录，则直接判定为满分全连
            totalScore = 1000000;
        score.text = Reformating(totalScore);
        panelScore.text = Reformating(totalScore);

        if (totalScore <= 600000)
        {
            panelLevel.text = "F";
            panelLevel.color = new Color(200,200,200);
        }else if(totalScore <= 750000)
        {
            panelLevel.text = "C";
            panelLevel.color = new Color(150, 210, 255);
        }else if (totalScore <= 880000)
        {
            panelLevel.text = "B";
            panelLevel.color = new Color(100, 200, 50);
        }else if (totalScore <= 950000)
        {
            panelLevel.text = "A";
            panelLevel.color = new Color(255, 100, 100);
        }else if (totalScore < 1000000)
        {
            panelLevel.text = "S";
            panelLevel.color = new Color(255, 210, 100);
        }else if (totalScore == 1000000)
        {
            panelLevel.text = "M";
            panelLevel.color = new Color(255, 180, 100);
        }
    }

    public void ComboReset()
    {
        curCombo = 0;
        combo.text = curCombo.ToString();
    }

    public void PerfectUp()
    {
        perfect++;
        panelPerfect.text = perfect.ToString();
    }

    public void GreatUp()
    {
        great++;
        panelGreat.text = great.ToString();
    }

    public void BadUp()
    {
        bad++;
        panelBad.text = bad.ToString();
    }

    public void MissUp()
    {
        miss++;
        panelMiss.text = miss.ToString();
    }

    public void ComboUp()
    {
        curCombo++;
        maxCombo = Mathf.Max(maxCombo, curCombo);
        panelMaxCombo.text = maxCombo.ToString();
        combo.text = curCombo.ToString();
    }

    public string Reformating(int scores)
    {
        string res = scores.ToString();
        while (res.Length < 7)
            res = "0" + res;
        return res;
    }
}
