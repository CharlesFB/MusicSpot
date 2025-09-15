using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mono.Data.Sqlite;

public class Initialize : MonoBehaviour
{
    public string[,] chapters = {
                { "0", "'动人国风'", "2", "2", "'暂未添加章节介绍'" },
                { "1", "'Speed Up!'", "3", "0", "'暂未添加章节介绍'"}
            };

    public string[,] musics = {
                { "0", "1", "'Cthugha'", "'USAO'", "0", "106.5", "0", "1", "2", "0", "3", "0" },
                { "1", "0", "'天道酬勤'", "'SNKS'", "1", "150", "0", "4", "5", "6", "7", "0"},
                { "2", "0", "'龍心'","'UnicornPhantom'","0", "130", "0","8","9","10","11","0"},
                { "3", "1", "'Originally'", "'SNKS'", "1", "100", "0", "12","13","14","15","0"},
                { "4", "1", "'Sakuravania'", "'UnicornPhantom'", "2", "75", "0","16","17","18","19","0" }
            };

    public string[,] scores = {
                { "0", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "15", "0"},
                { "1", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "5", "0"},
                { "2", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "9", "0"},
                { "3", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "16", "0"},
                { "4", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "4", "0"},
                { "5", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "9", "0"},
                { "6", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "14", "0"},
                { "7", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "16", "0"},
                { "8", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "5", "0"},
                { "9", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "7", "0"},
                { "10", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "10", "0"},
                { "11", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "11", "0"},
                { "12", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "7", "0"},
                { "13", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "12", "0"},
                { "14", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "15", "0"},
                { "15", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "18", "0"},
                { "16", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "6", "0"},
                { "17", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "9", "0"},
                { "18", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "13", "0"},
                { "19", "0", "1", "400", "400", "700", "50", "1000", "'0000000'", "15", "0"}
            };

    private SqliteDataReader dataReader;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        //1. 打开/创建名为“MusicSpot”的数据库(单例中打开)

        CreateChapter();  //2. 如果之前不存在章节表（Chapter），则创建
        CreateMusic();  //3. 如果之前不存在音乐表（Music），则创建
        CreateScore();  //4. 如果之前不存在谱面表（Score），则创建

        DatabaseSingleton.instance.GenerateAllChapters();
        DatabaseSingleton.instance.GenerateAllMusics();
        DatabaseSingleton.instance.GenerateAllScores();

    }

    private void CreateChapter()
    {
        string[] col = { "Id", "ChapterName", "MusicNum", "LastPlayedId", "Info" };
        string[] colType = { "INT", "CHAR", "INT", "INT", "CHAR" };
        DatabaseSingleton.instance.CreateTable("Chapter", col, colType);
        dataReader = DatabaseSingleton.instance.QuaryData("Chapter");
        if (!dataReader.Read())
        {
            //新章节表，插入默认章节列表数据
            DatabaseSingleton.instance.InsertInto("Chapter", chapters);
            Debug.Log("插入章节结束");
        }
    }

    private void CreateMusic()
    {
        string[] col = { "Id", "ChapterId", "MusicName", "Author", "ClipId", "Bpm", "LastPlayedDiff", "EasyId", "NormalId", "HardId", "InsaneId", "SpecialId" };
        string[] colType = { "INT", "INT", "CHAR", "CHAR", "INT", "FLOAT", "INT", "INT", "INT", "INT", "INT", "INT" };
        DatabaseSingleton.instance.CreateTable("Music", col, colType);
        dataReader = DatabaseSingleton.instance.QuaryData("Music");

        if (!dataReader.Read())
        {
            DatabaseSingleton.instance.InsertInto("Music", musics);
            Debug.Log("插入乐曲结束");
        }
    }

    private void CreateScore()
    {
        string[] col = { "Id", "MusicId","Beat","SinglePerfectScore","TouchPerfectScore","PressPerfectScore","ExtraScore","TotalCombo", "Score", "Diff", "MaxCombo" };
        string[] colType = { "INT", "INT", "INT", "INT", "INT", "INT", "INT", "INT", "CHAR", "INT", "INT" };
        DatabaseSingleton.instance.CreateTable("Score", col, colType);
        dataReader = DatabaseSingleton.instance.QuaryData("Score");

        if (!dataReader.Read())
        {
            DatabaseSingleton.instance.InsertInto("Score", scores);
            Debug.Log("插入谱面结束");
        }
    }
}
