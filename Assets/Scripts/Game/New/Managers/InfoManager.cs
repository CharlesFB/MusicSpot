using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class InfoManager : MonoBehaviour
{
    public Sprite[] backs;
    public AudioClip[] clips;
    public Koreography[] koreographies;

    public Text musicInfo;
    public Text difficulty;
    public Text score;
    public Text combo;
    public Image background;
    public Text panelDiff;
    public Text panelInfo;

    public AudioSource musicManager; //给musicManager乐曲
    public ScoreManager scoreManager; //给谱面Manager谱面注册信息

    public float speedFec; //基准速度乘以此值；基准时间间隔除以此值
    public float secPerBeat; //每拍时间（单位：秒），用bpm可以直接获得 :  60/bpm

    void Awake()
    {
        SetInfo();
        MusicInfoOn();
        SetSpeed();
    }

    private void SetInfo()
    {
        string musicInfoString = GameInfoSingleton.instance.author + " - " + GameInfoSingleton.instance.musicName; //设置乐曲信息（作者+曲名称）
        musicInfo.text = musicInfoString;
        panelInfo.text = musicInfoString;
        difficulty.text = GameInfoSingleton.instance.diff; //设置谱面难度信息
        panelDiff.text = GameInfoSingleton.instance.diff; //设置结算界面难度信息
    }

    private void MusicInfoOn() //设置乐曲背景和曲目信息
    {
        musicManager.clip = clips[GameInfoSingleton.instance.musicId]; 
        background.sprite = backs[GameInfoSingleton.instance.musicId];
        scoreManager.SetKoreographer(koreographies[GameInfoSingleton.instance.musicId]);
        string registerName = GameInfoSingleton.instance.musicName + "_" + GameInfoSingleton.instance.diff;
        scoreManager.registerBuilder = registerName; //曲目注册名 “乐曲名_难度”；判定线：“乐曲名_难度_Line”；判定区域：“乐曲名_难度_Area”；判定边界线：“乐曲名_难度_Line1”,“乐曲名_难度_Line2”
    }

    private void SetSpeed() //设置曲目速度信息
    {
        speedFec = GameInfoSingleton.instance.bpm / 100 * 1 / GameInfoSingleton.instance.beat; // fec = bpm/100 * 1/beat 以（100bpm,1拍子）速度为基准
        secPerBeat = 60 / GameInfoSingleton.instance.bpm;
    }
}
