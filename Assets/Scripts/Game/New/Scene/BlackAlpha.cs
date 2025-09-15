using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackAlpha : MonoBehaviour
{
    public bool isOkForClick;
    public ResultManager result;
    public ScenesBlackFade black;

    private void Start()
    {
        isOkForClick = false;
    }

    private void Update()
    {
        if (isOkForClick)
        {
            int count = Input.touchCount;
            for (int i = 0; i < count; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                    OnClickThis();
                break;
            }
            
        }
    }

    public void OnClickThis()
    {
        switch (VarSingleton.instance.diff)
        {
            case 0:
                DatabaseSingleton.instance.UpdateTable("Score", "Score", result.Reformating(result.totalScore), "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId);
                DatabaseSingleton.instance.UpdateTable("Score", "MaxCombo", result.maxCombo, "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId);
                break;
            case 1:
                DatabaseSingleton.instance.UpdateTable("Score", "Score", result.Reformating(result.totalScore), "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId);
                DatabaseSingleton.instance.UpdateTable("Score", "MaxCombo", result.maxCombo, "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId);
                break;
            case 2:
                DatabaseSingleton.instance.UpdateTable("Score", "Score", result.Reformating(result.totalScore), "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId);
                DatabaseSingleton.instance.UpdateTable("Score", "MaxCombo", result.maxCombo, "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId);
                break;
            case 3:
                DatabaseSingleton.instance.UpdateTable("Score", "Score", result.Reformating(result.totalScore), "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId);
                DatabaseSingleton.instance.UpdateTable("Score", "MaxCombo", result.maxCombo, "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId);
                break;
            case 4:
                DatabaseSingleton.instance.UpdateTable("Score", "Score", result.Reformating(result.totalScore), "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId);
                DatabaseSingleton.instance.UpdateTable("Score", "MaxCombo", result.maxCombo, "Id", VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId);
                break;
        }
        black.blackFadeIn();
        Invoke("LoadChapter", 0.5f);
    }

    private void LoadChapter()
    {
        SceneManager.LoadScene("ChapterPage");
    }
}
