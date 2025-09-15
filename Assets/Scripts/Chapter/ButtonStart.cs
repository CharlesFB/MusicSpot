using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ButtonStart : MonoBehaviour
{

    public ScenesBlackFade black;

    public void OnClickedStart()
    {
        GameInfoSingleton.instance.musicName = VarSingleton.instance.musics[VarSingleton.instance.musicNum].musicName;
        GameInfoSingleton.instance.musicId = VarSingleton.instance.musicNum;
        GameInfoSingleton.instance.author = VarSingleton.instance.musics[VarSingleton.instance.musicNum].author;
        switch (VarSingleton.instance.diff)
        {
            case 0:
                GameInfoSingleton.instance.diff = "Easy";
                GameInfoSingleton.instance.beat = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].beat;
                GameInfoSingleton.instance.singlePerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].singlePerfectScore;
                GameInfoSingleton.instance.touchPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].touchPerfectScore;
                GameInfoSingleton.instance.pressPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].pressPerfectScore;
                GameInfoSingleton.instance.extraScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].extraScore;
                GameInfoSingleton.instance.totalCombo = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].easyId].totalCombo;
                break;
            case 1:
                GameInfoSingleton.instance.diff = "Normal";
                GameInfoSingleton.instance.beat = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].beat;
                GameInfoSingleton.instance.singlePerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].singlePerfectScore;
                GameInfoSingleton.instance.touchPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].touchPerfectScore;
                GameInfoSingleton.instance.pressPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].pressPerfectScore;
                GameInfoSingleton.instance.extraScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].extraScore;
                GameInfoSingleton.instance.totalCombo = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].normalId].totalCombo;
                break;
            case 2:
                GameInfoSingleton.instance.diff = "Hard";
                GameInfoSingleton.instance.beat = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].beat;
                GameInfoSingleton.instance.singlePerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].singlePerfectScore;
                GameInfoSingleton.instance.touchPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].touchPerfectScore;
                GameInfoSingleton.instance.pressPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].pressPerfectScore;
                GameInfoSingleton.instance.extraScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].extraScore;
                GameInfoSingleton.instance.totalCombo = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].hardId].totalCombo;
                break;
            case 3:
                GameInfoSingleton.instance.diff = "Insane";
                GameInfoSingleton.instance.beat = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].beat;
                GameInfoSingleton.instance.singlePerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].singlePerfectScore;
                GameInfoSingleton.instance.touchPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].touchPerfectScore;
                GameInfoSingleton.instance.pressPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].pressPerfectScore;
                GameInfoSingleton.instance.extraScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].extraScore;
                GameInfoSingleton.instance.totalCombo = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].insaneId].totalCombo;
                break;
            case 4:
                GameInfoSingleton.instance.diff = "Special";
                GameInfoSingleton.instance.beat = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].beat;
                GameInfoSingleton.instance.singlePerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].singlePerfectScore;
                GameInfoSingleton.instance.touchPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].touchPerfectScore;
                GameInfoSingleton.instance.pressPerfectScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].pressPerfectScore;
                GameInfoSingleton.instance.extraScore = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].extraScore;
                GameInfoSingleton.instance.totalCombo = VarSingleton.instance.scores[VarSingleton.instance.musics[VarSingleton.instance.musicNum].specialId].totalCombo;
                break;
        }
        GameInfoSingleton.instance.bpm = VarSingleton.instance.musics[VarSingleton.instance.musicNum].bpm;

        black.blackFadeIn();
        Invoke("LoadGame", 0.5f);
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
}
