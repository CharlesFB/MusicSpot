using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisibleMusic : MonoBehaviour
{
    public int chapterNumber; //属于第几章

    public int count = 128*2;
    public GameObject barPrefab;
    public float angleGap = 360f / 256; //分摊360度

    private AudioSource audioSource; // 音源

    private float[] samples;
    private List<SpriteRenderer> barSprites = new List<SpriteRenderer>();
    [Range(1, 30)]
    public float UpLerp = 12;

    [Range(0.0006f,0.01f)]
    public float indexFec = 0.005f;
    [Range(1,100)]
    public float init = 15;

    private int lastPlayedChapter;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("ChapterMusicPlayer").GetComponent<AudioSource>();
        angleGap = 360f / count;
        samples = new float[count];
        for (int i = 0; i < count; i++)
        {
            GameObject bar = Instantiate(barPrefab,this.transform);
            bar.name = string.Format("MusicBar[{0}]", i+1);
            barSprites.Add(bar.GetComponent<SpriteRenderer>());
            bar.transform.rotation = Quaternion.Euler(0,0,angleGap*i);
        }
        lastPlayedChapter = VarSingleton.instance.chapterNum;

    }

    // Update is called once per frame
    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        for (int i = 0; i < barSprites.Count; i++)
        {
            Vector3 _v3 = barSprites[i].transform.localScale;
            _v3 = new Vector3( Mathf.Clamp(samples[i] * (init + i * i * indexFec + i * indexFec * 1.6f), 0.33f, 0.49f),0.1f, 1);
            barSprites[i].transform.localScale = Vector3.Lerp(barSprites[i].transform.localScale, _v3, Time.deltaTime * UpLerp);
        }

        if (lastPlayedChapter != VarSingleton.instance.chapterNum)
        {//回到章节菜单，关闭可视化
            lastPlayedChapter = VarSingleton.instance.chapterNum;
            if (VarSingleton.instance.chapterNum == -1)
                transform.localScale = new Vector3(0, 1, 1);//把一x设到0
        }
    }
}
