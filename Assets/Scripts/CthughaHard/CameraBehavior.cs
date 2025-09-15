using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
    private AudioSource aud;
    private Text scoreText;
    private Text comboText;
    private Animator ani;

    public string eventID;
    public GameObject SinglePrefab;
    public GameObject TouchPrefab;
    public GameObject PressPrefab;
    public int TOTAL_SPOT = 1000;
    public float extraScoreRate = 100;

    private int layer;
    private int totalScore;
    private int totalCombo;
    private int maxCombo;
    private int curCombo;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        scoreText = GameObject.Find("Scores").GetComponent<Text>();
        comboText = GameObject.Find("CountCombo").GetComponent<Text>();
        ani = GameObject.Find("CountCombo").GetComponent<Animator>();
        aud.Stop();

        layer = 1000;
        totalScore = 0;
        totalCombo = 0;
        maxCombo = 0;
        curCombo = 0;

        Invoke("audioPlay", 4); //延迟4秒播放
        Koreographer.Instance.RegisterForEvents(eventID, SpotProduce);

    }

    void Update()
    {
		
	}

    public void audioPlay()
    {
        aud.Play();
    }

    public void audioPause()
    {
        aud.Pause();
    }

    public void audioReset()
    {
        aud.Stop();
        aud.Play();
    }

    //存放音符的信息
    private class SpotInfo
    {
        public int type;
        public float posX;
        public float posY;
        public int dir;
        public int len;
        public SpotInfo(int type,float posX,float posY,int dir,int len)
        {
            this.type = type;
            this.posX = posX;
            this.posY = posY;
            this.dir = dir;
            this.len = len;
        }
        public SpotInfo() { }
    }

    // 生产音符
    private void SpotProduce(KoreographyEvent koreographyEvent)
    {
        if (koreographyEvent != null)
        {
            SpotInfo[] spotInfos = GetInfo(koreographyEvent.GetTextValue());
            for (int i = 1; i <= spotInfos[0].len; i++)
            {
                switch (spotInfos[i].type)
                {
                    case 0: // 单击音符
                        GameObject tmp = Instantiate(SinglePrefab);
                        tmp.transform.position = new Vector3(spotInfos[i].posX, spotInfos[i].posY);
                        tmp.GetComponent<SpriteRenderer>().sortingOrder = layer--;
                        tmp.GetComponent<SingleSpot>().timeBegin = aud.time;
                        tmp.transform.parent = GameObject.Find("Plane").transform;
                        break;
                    case 1: // 触发音符
                        GameObject touch = Instantiate(TouchPrefab);
                        touch.transform.position = new Vector3(spotInfos[i].posX, spotInfos[i].posY);
                        touch.GetComponent<SpriteRenderer>().sortingOrder = layer--;
                        touch.transform.parent = GameObject.Find("Plane").transform;
                        break;
                    case 2: // 蓄力音符
                        GameObject press = Instantiate(PressPrefab);
                        press.transform.position = new Vector3(spotInfos[i].posX, spotInfos[i].posY);
                        press.GetComponent<SpriteRenderer>().sortingOrder = layer--;
                        press.transform.parent = GameObject.Find("Plane").transform;
                        press.GetComponent<PressSpot>().init(spotInfos[i].dir, spotInfos[i].len);
                        break;
                }

            }
        }

    }

    // 转换负载的文本
    /*
     * 10142034802,10181123411
     * -> [0] len  = 2
     * 
     *    [1] type = 1
     *        posX = 1.42
     *        posY = 3.48
     *        dir  = 0
     *        len  = 2
     *        
     *    [2] type = 1
     *        posX = 1.81
     *        posY = -2.34
     *        dir  = 1
     *        len  = 1
     */
    private SpotInfo[] GetInfo(string s)
    {
        SpotInfo[] res = new SpotInfo[10];
        res[0] = new SpotInfo();
        int cnt = 0;
        string[] infos = s.Split(',');
        foreach(string info in infos)
        {
            Debug.Log(info);
            long x = long.Parse(info);
            cnt++;
            res[cnt] = new SpotInfo();
            res[cnt].len = (int)(x % 10);
            x /= 10;
            res[cnt].dir = (int)(x % 10);
            x /= 10;
            res[cnt].posY = (x % 1000) / 100.0f;
            x /= 1000;
            if (x % 10 == 1)
                res[cnt].posY *= -1;
            x /= 10;
            res[cnt].posX = (x % 1000) / 100.0f;
            x /= 1000;
            if (x % 10 == 1)
                res[cnt].posX *= -1;
            x /= 10;
            res[cnt].type = (int)x;
            //Debug.Log(res[cnt]);
        }
        res[0].len = cnt;
        return res;
    } 

    public void ScoreUp(int s)
    {
        totalScore += s + (int)(curCombo / (float)TOTAL_SPOT * extraScoreRate);
        if (totalScore > 100000)
            scoreText.text = "100000";
        else 
            scoreText.text = Converting(totalScore);
    }

    public void ComboReset()
    {
        curCombo = 0;
        comboText.text = curCombo.ToString();
    }

    public void ComboUp()
    {
        curCombo++;
        totalCombo++;
        maxCombo = Mathf.Max(maxCombo, curCombo);
        comboText.text = curCombo.ToString();
        //Debug.Log("-=======-");
    }

    private string Converting(int num)
    {
        string res = num.ToString();
        while (res.Length < 6)
            res = "0" + res;
        return res;
    }
}

