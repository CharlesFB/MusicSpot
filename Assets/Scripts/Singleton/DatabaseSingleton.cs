using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DatabaseSingleton : MonoBehaviour
{
    private SqliteConnection connection;
    private SqliteCommand cmd;
    private SqliteDataReader dataReader;

    private bool isIn = false;
    private string dbPath = "";

    public Text debug1;
    public Text debug2;
    public Text debug3;
    public Text debug4;

    public static DatabaseSingleton instance; //单例模式

    [Obsolete]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            if (Application.platform != RuntimePlatform.Android)
            {
                dbPath = Application.dataPath + "/MusicSpot.db";
                OpenSQLConnection("data source=" + dbPath);
            }
            else //Android
            {
                dbPath = Application.persistentDataPath + "/MusicSpot.sqlite";

                if (!Directory.Exists(Application.persistentDataPath))
                {
                    Directory.CreateDirectory(Application.persistentDataPath);
                }

                //DebugOnScreen(dbPath);
                string streamingPath = Path.Combine(Application.streamingAssetsPath, "MusicSpot.sqlite");

                try
                {
                    WWW loadWWW = new WWW(streamingPath);

                    if (!File.Exists(dbPath))
                    {
                        while (!loadWWW.isDone)
                        {
                        }

                        File.WriteAllBytes(dbPath, loadWWW.bytes);
                    }

                    OpenSQLConnection("URI=file:" + dbPath);

                }
                catch (Exception e)
                {

                }
                
            }
               
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            CloseSQLConnection();
            Destroy(gameObject);
        }
    }

    private void DebugOnScreen(string text, int x)
    {
        switch (x)
        {
            case 1:
                debug1.text = text;
                break;
            case 2:
                debug2.text = text;
                break;
            case 3:
                debug3.text = text;
                break;
            case 4:
                debug4.text = text;
                break;
        }
    }


    public void OpenSQLConnection(string dbObject)
    {
        Debug.Log("进入连接方法");
        //DebugOnScreen("Connecting", 3);
        try
        {
            connection = new SqliteConnection(dbObject);
            connection.Open();
            Debug.Log("数据库：" + dbObject + "连接成功");
            //DebugOnScreen("succeed", 4);
        }
        catch (Exception e)
        {
            Debug.LogError("异常错误：" + e.ToString());
            //DebugOnScreen(e.ToString(), 1);
        }
    }

    public void CloseSQLConnection()
    {
        if (cmd != null)
        {
            //清空cmd命令内容
            cmd.Dispose();
            cmd = null;
        }
        if (dataReader != null)
        {
            dataReader.Dispose();
            dataReader = null;
        }
        if (connection != null)
        {
            connection.Dispose();
            connection = null;
        }
        Debug.Log("数据库已断开连接！");
    }

    public SqliteDataReader ExecuteCommand(string sqlQuery)
    {
        try
        {
            cmd = connection.CreateCommand();
        }catch(SqliteException e)
        {
            DebugOnScreen(e.ToString(), 2);
        }
        
        cmd.CommandText = sqlQuery;
        

        try
        {
            dataReader = cmd.ExecuteReader();
        }catch(Exception e)
        {
            DebugOnScreen(sqlQuery, 3);
            DebugOnScreen(e.ToString(), 1);
        }
        
        //isIn = true;

        return dataReader;
    }

    public void InsertInto(string tableName, string[,] values) //插入
    {
        //Debug.Log("进入插入方法");

        for (int i = 0; i < values.GetLength(0); i++)
        {
            string query = "INSERT INTO \"" + tableName + "\" VALUES ";

            query += " (" + values[i, 0];
            for (int j = 1; j < values.GetLength(1); j++)
            {
                query += ", " + values[i, j];
            }
            query += ");";
            ExecuteCommand(query);
        }
    }

    public SqliteDataReader CreateTable(string tabName, string[] col, string[] colType) //建表
    {
        if (col.Length != colType.Length)
        {
            throw new SqliteException("columns.Length != colType.Length");
        }

        //ExecuteCommand("DROP table "+tabName);

        string query = "CREATE TABLE IF NOT EXISTS " + tabName + " (" + col[0] + " " + colType[0];

        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }

        query += ")";

        

        return ExecuteCommand(query);
    }

    public SqliteDataReader QuaryData(string tableName)   //读表
    {
        string query = "SELECT * FROM " + tableName;

        return ExecuteCommand(query);
    }

    public SqliteDataReader QuaryData(string tableName, string fieldName)  //读一列
    {
        string query = "SELECT " + fieldName + " FROM " + tableName;

        return ExecuteCommand(query);
    }

    public SqliteDataReader QuaryData(string tableName, string conditionField, string condition) //按一个条件
    {
        string query = "SELECT * FROM " + tableName + " WHERE " + conditionField + " = " + condition;

        return ExecuteCommand(query);
    }

    public string ColToString(SqliteDataReader reader, string colName)
    {
        return reader[colName].ToString();
    }

    public int ColToInt(SqliteDataReader reader, string colName)
    {
        //Debug.Log(colName);
        if (colName == "")
            return -1;
        return int.Parse(ColToString(reader, colName));
    }

    public float ColToFloat(SqliteDataReader reader, string colName)
    {
        //Debug.Log(colName);
        if (colName == "")
            return -1;
        return float.Parse(ColToString(reader, colName));
    }

    /*
     * 获取全部章节列表，存入VarSingleton.instance.chapters中
     */
    public void GenerateAllChapters()
    {
        dataReader = QuaryData("Chapter"); //读章节表内全部信息
        int count = 0;
        while (dataReader.Read())
        {
            //Debug.Log("Count++");
            count++;
            //Debug.Log(VarSingleton.instance.chapters.Length);
            VarSingleton.instance.chapters[ColToInt(dataReader, "Id")] = new VarSingleton.Chapter();
            VarSingleton.instance.chapters[ColToInt(dataReader, "Id")].chapterName = ColToString(dataReader, "ChapterName");
            VarSingleton.instance.chapters[ColToInt(dataReader, "Id")].musicNum = ColToInt(dataReader, "MusicNum");
            VarSingleton.instance.chapters[ColToInt(dataReader, "Id")].lastPlayedId = ColToInt(dataReader, "LastPlayedId");
            VarSingleton.instance.chapters[ColToInt(dataReader, "Id")].info = ColToString(dataReader, "Info");
        }
        VarSingleton.instance.chapterCount = count;
    }

    /*
     * 获取全部乐曲信息，存入VarSingleton.instance.musics中
     */
    public void GenerateAllMusics()
    {
        dataReader = QuaryData("Music");
        while (dataReader.Read())
        {
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")] = new VarSingleton.Music();
           
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].id = ColToInt(dataReader, "Id");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].chapterId = ColToInt(dataReader, "ChapterId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].musicName = ColToString(dataReader, "MusicName");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].author = ColToString(dataReader, "Author");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].clipId = ColToInt(dataReader, "ClipId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].bpm = ColToFloat(dataReader, "Bpm");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].lastPlayedDiff = ColToInt(dataReader, "LastPlayedDiff");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].easyId = ColToInt(dataReader, "EasyId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].normalId = ColToInt(dataReader, "NormalId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].hardId = ColToInt(dataReader, "HardId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].insaneId = ColToInt(dataReader, "InsaneId");
            VarSingleton.instance.musics[ColToInt(dataReader, "Id")].specialId = ColToInt(dataReader, "SpecialId");
           // Debug.Log(ColToInt(dataReader, "Id").ToString() + "===ID");
        }
    }

    /*
     * 获取全部的谱面信息，存入VarSingleton.instance.score
     */
    public void GenerateAllScores()
    {
        dataReader = QuaryData("Score");
        while (dataReader.Read())
        {
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")] = new VarSingleton.Score();

            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].id = ColToInt(dataReader, "Id");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].musicId = ColToInt(dataReader, "MusicId");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].beat = ColToInt(dataReader, "Beat");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].singlePerfectScore = ColToInt(dataReader, "SinglePerfectScore");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].touchPerfectScore = ColToInt(dataReader, "TouchPerfectScore");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].pressPerfectScore = ColToInt(dataReader, "PressPerfectScore");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].extraScore = ColToInt(dataReader, "ExtraScore");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].totalCombo = ColToInt(dataReader, "TotalCombo");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].score = ColToString(dataReader, "Score");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].diff = ColToInt(dataReader, "Diff");
            VarSingleton.instance.scores[ColToInt(dataReader, "Id")].maxCombo = ColToInt(dataReader, "MaxCombo");
        }
    }

    /*
     * 修改表的某一项（行列标识）
     */
    public void UpdateTable(string tableName, string col, string colData, string row, string rowCondition)
    {
        string query = "UPDATE " + tableName + " SET " + col + " = '" + colData + "' WHERE "
            + row + " = '" + rowCondition + "'";
        ExecuteCommand(query);
    }
    public void UpdateTable(string tableName, string col, string colData, string row, int rowCondition)
    {
        string query = "UPDATE " + tableName + " SET " + col + " = '" + colData + "' WHERE "
            + row + " = " + rowCondition.ToString() + "";
        ExecuteCommand(query);
    }
    public void UpdateTable(string tableName, string col, int colData, string row, string rowCondition)
    {
        string query = "UPDATE " + tableName + " SET " + col + " = " + colData.ToString() + " WHERE "
            + row + " = '" + rowCondition + "'";
        ExecuteCommand(query);
    }
    public void UpdateTable(string tableName, string col, int colData, string row, int rowCondition)
    {
        string query = "UPDATE " + tableName + " SET " + col + " = " + colData.ToString() + " WHERE "
            + row + " = " + rowCondition.ToString();
        ExecuteCommand(query);
    }

}
