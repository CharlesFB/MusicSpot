using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoSingleton : MonoBehaviour
{

    public static GameInfoSingleton instance; //单例模式
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    //游戏前

    public string musicName = "Originally";
    public string author = "SNKS";
    public string diff = "Hard";
    public int musicId = 3;//音乐编号，用于MusicMananger播放;同时用于背景图改变
    public float bpm = 100;
    public int beat = 1;
    public int singlePerfectScore = 400;
    public int touchPerfectScore = 300;
    public int pressPerfectScore = 700;
    public int extraScore = 100;
    public int totalCombo = 290; //谱面的七个关键信息

    //游戏中
    public bool isPaused = false;
    public float[] gaps = {0.9f, 1.07f, 1.2f, 1.42f, 1.48f, 1.54f}; //100bpm, 1拍曲目的基准数据(single)

    //结算后

    public string score;
    public string level;//评级
    public int maxCombo;
}
