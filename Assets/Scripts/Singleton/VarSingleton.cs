using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarSingleton : MonoBehaviour
{
    public static VarSingleton instance; //单例模式
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

    public float chapterGap = 25; //章节间间隔
    public int chapterCount; //章节数
    public int chapterNum;  //当前选中章节编号，没有-1
    public int musicNum; //当前选中乐曲编号，没有-1
    public int musicNumInChapter; //当前乐曲在章节内的编号， 没有-1
    public int diff; //当前选中乐曲难度：0:easy; 1:normal; 2:hard; 3:insane; 4:special。默认0
    public Chapter[] chapters;
    public Music[] musics; //全部音乐
    public Score[] scores;

    public class Chapter
    {
        public string chapterName;
        public int musicNum;
        public int lastPlayedId;
        public string info;

        public Chapter() { }
        public Chapter(string chapterName, int musicNum, int lastPlayedId, string info)
        {
            this.chapterName = chapterName;
            this.musicNum = musicNum;
            this.lastPlayedId = lastPlayedId;
            this.info = info;
        }
    }

    public class Music //每一章的音乐
    {
        public int id;
        public int chapterId;
        public string musicName;
        public string author;
        public int clipId;
        public float bpm;
        public int lastPlayedDiff;
        public int easyId;
        public int normalId;
        public int hardId;
        public int insaneId;
        public int specialId;

        public Music() { }
        
    }

    public class Score
    {
        public int id;
        public int musicId;
        public int beat;
        public int singlePerfectScore;
        public int touchPerfectScore;
        public int pressPerfectScore;
        public int extraScore;
        public int totalCombo;
        public string score;
        public int diff;
        public int maxCombo;

        public Score() { }
    }

    public string changeScoreFormat(string score)
    {
        string sco = score;
        while (sco.Length <= 7)
            sco = "0" + sco;
        return sco;
    }

    void Start()
    {
        chapterNum = -1;
        musicNum = -1;
        musicNumInChapter = -1;
        diff = 0;

        chapters = new Chapter[10];
        musics = new Music[100];
        scores = new Score[500];
    }
}
