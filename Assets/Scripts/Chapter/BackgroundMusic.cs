using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip defaultMusic; //默认背景音乐
    public AudioClip[] musics; //乐曲片段

    private int lastMusic; //记录上一帧的音乐编号，-1为默认

    // Start is called before the first frame update
    void Start()
    {
        lastMusic = VarSingleton.instance.musicNum;
        if (lastMusic == -1)
            AlterClip(defaultMusic);
        else
            AlterClip(musics[lastMusic]);
    }

    // Update is called once per frame
    void Update()
    {
        if (VarSingleton.instance.musicNum != lastMusic) //检测到音乐变化
        {
            lastMusic = VarSingleton.instance.musicNum;
            if (lastMusic == -1)
            {
                AlterClip(defaultMusic);
            }
            else
            {
                AlterClip(musics[lastMusic]);
            }

        }
    }

    void AlterClip(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

}
