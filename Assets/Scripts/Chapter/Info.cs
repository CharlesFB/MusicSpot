using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Info : MonoBehaviour
{
    public GameObject chapterInfo;
    public GameObject musicInfo;
    public GameObject chapterName;
    public GameObject musicName;
    public GameObject author;
    public GameObject easy;
    public GameObject normal;
    public GameObject hard;
    public GameObject insane;
    public GameObject diff;
    public GameObject maxCombo;
    public GameObject score;
    public GameObject grade;

    public void SetDiffAndScores(int diffCode)
    {
        //修改数据库上次游玩难度的信息
        DatabaseSingleton.instance.UpdateTable("Music", "LastPlayedDiff", diffCode, "Id", VarSingleton.instance.musicNum);
        VarSingleton.Music music = VarSingleton.instance.musics[VarSingleton.instance.musicNum];
        music.lastPlayedDiff = diffCode;
        int sgrade = 0;
        switch (diffCode)
        {
            case 0:
                VarSingleton.instance.diff = 0; //修改公有变量
                easy.GetComponent<Toggle>().isOn = true;
                diff.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.easyId].diff.ToString();
                score.GetComponent<Text>().text = VarSingleton.instance.changeScoreFormat(
                    VarSingleton.instance.scores[music.easyId].score);
                sgrade = int.Parse(VarSingleton.instance.scores[music.easyId].score);
                maxCombo.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.easyId].maxCombo.ToString();
                break;
            case 1:
                VarSingleton.instance.diff = 1;
                normal.GetComponent<Toggle>().isOn = true;
                diff.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.normalId].diff.ToString();
                score.GetComponent<Text>().text = VarSingleton.instance.changeScoreFormat(
                    VarSingleton.instance.scores[music.normalId].score);
                sgrade = int.Parse(VarSingleton.instance.scores[music.normalId].score);
                maxCombo.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.normalId].maxCombo.ToString();
                break;
            case 2:
                VarSingleton.instance.diff = 2;
                hard.GetComponent<Toggle>().isOn = true;
                diff.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.hardId].diff.ToString();
                score.GetComponent<Text>().text = VarSingleton.instance.changeScoreFormat(
                    VarSingleton.instance.scores[music.hardId].score);
                sgrade = int.Parse(VarSingleton.instance.scores[music.hardId].score);
                maxCombo.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.hardId].maxCombo.ToString();
                break;
            case 3:
                VarSingleton.instance.diff = 3;
                insane.GetComponent<Toggle>().isOn = true;
                diff.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.insaneId].diff.ToString();
                score.GetComponent<Text>().text = VarSingleton.instance.changeScoreFormat(
                    VarSingleton.instance.scores[music.insaneId].score);
                sgrade = int.Parse(VarSingleton.instance.scores[music.insaneId].score);
                maxCombo.GetComponent<Text>().text =
                    VarSingleton.instance.scores[music.insaneId].maxCombo.ToString();
                break;
            default:
                break;
        }
        if (sgrade == 100000)
        {
            grade.GetComponent<Text>().text = "S";
            grade.GetComponent<Text>().color = Color.yellow;
        }else if(sgrade >= 90000)
        {
            grade.GetComponent<Text>().text = "A";
            grade.GetComponent<Text>().color = Color.red;
        }else if (sgrade >= 82000)
        {
            grade.GetComponent<Text>().text = "B";
            grade.GetComponent<Text>().color = Color.green;
        }else if (sgrade >= 70000)
        {
            grade.GetComponent<Text>().text = "C";
            grade.GetComponent<Text>().color = Color.blue;
        }else
        {
            grade.GetComponent<Text>().text = "F";
            grade.GetComponent<Text>().color = Color.gray;
        }

        //设置渐显动画
        Text diffText = diff.GetComponent<Text>();
        diffText.color = new Color(diffText.color.r, diffText.color.g, diffText.color.b, 0);
        diffText.DOFade(1, 0.6f);
        Text maxComboText = maxCombo.GetComponent<Text>();
        maxComboText.color = new Color(maxComboText.color.r, maxComboText.color.g, maxComboText.color.b, 0);
        maxComboText.DOFade(1, 0.6f);
        Text scoreText = score.GetComponent<Text>();
        scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0);
        scoreText.DOFade(1, 0.6f);
        Text gradeText = grade.GetComponent<Text>();
        gradeText.color = new Color(gradeText.color.r, gradeText.color.g, gradeText.color.b, 0);
        gradeText.DOFade(1, 0.6f);
    }

    public void SetName()
    {
        VarSingleton.Chapter chapter = VarSingleton.instance.chapters[VarSingleton.instance.chapterNum];
        VarSingleton.Music music = VarSingleton.instance.musics[VarSingleton.instance.musicNum];
        chapterName.GetComponent<Text>().text = chapter.chapterName;
        //Debug.Log(VarSingleton.instance.musicNum);
        musicName.GetComponent<Text>().text = music.musicName;
        author.GetComponent<Text>().text = music.author;
    }

    public void MoveIn()
    {
        RectTransform rect1 = chapterInfo.GetComponent<RectTransform>();
        rect1.DOBlendableLocalMoveBy(new Vector3(824,0,420),0.8f);
        rect1.DOBlendableLocalRotateBy(new Vector3(0,-20,0),0.8f);
        RectTransform rect2 = musicInfo.GetComponent<RectTransform>();
        rect2.DOBlendableLocalRotateBy(new Vector3(0, 20, 0), 0.8f);
        rect2.DOBlendableLocalMoveBy(new Vector3(-824, 0, 420), 0.8f);
    }

    public void MoveOut()
    {
        if (VarSingleton.instance.chapterNum != -1)
        {
            RectTransform rect1 = chapterInfo.GetComponent<RectTransform>();
            rect1.DOBlendableLocalMoveBy(new Vector3(-824, 0, -420), 0.8f);
            rect1.DOBlendableLocalRotateBy(new Vector3(0, -70, 0), 0.8f);
            RectTransform rect2 = musicInfo.GetComponent<RectTransform>();
            rect2.DOBlendableLocalRotateBy(new Vector3(0, 70, 0), 0.8f);
            rect2.DOBlendableLocalMoveBy(new Vector3(824, 0, -420), 0.8f);
        }
    }
}
