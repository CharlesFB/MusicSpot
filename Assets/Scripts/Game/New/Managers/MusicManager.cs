using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    public Image blackAlpha;
    public RectTransform finalPanel;
    public AudioClip endMusic;
    public PauseManager pauseManager;

    private bool playing; //代表游戏曲目正在进行，暂停不会使其变为false；用于区分结束音乐播放

    private void Awake()
    {
        playing = false;
    }

    private void Start()
    {
        Invoke("AudioPlay", 3.5f);
    }

    private void Update()
    {
        if (playing == true && GetComponent<AudioSource>().isPlaying == false) //表示游戏曲目播放完了，该进入结算了
        {
            if (GetComponent<AudioSource>().time < GetComponent<AudioSource>().clip.length)
            {
                //表示为暂停
            }
            else
            { //结算
                pauseManager.pauseButton.interactable = false;
                blackAlpha.gameObject.GetComponent<BlackAlpha>().isOkForClick = true;
                Invoke("GetBack", 3f);
                FinalPanelMoveIn();
                playing = false;
                GetComponent<AudioSource>().clip = endMusic;
                GetComponent<AudioSource>().volume = 0.6f;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void GetBack()
    {
        blackAlpha.gameObject.GetComponent<BlackAlpha>().OnClickThis();
    }

    public void AudioPlay()
    {
        GetComponent<AudioSource>().Play();
        playing = true;
    }

    public void AudioPause()
    {
        GetComponent<AudioSource>().Pause();
    }

    public void FinalPanelMoveIn()
    {
        finalPanel.DOLocalMove(new Vector3(0,0,0),0.8f);
        blackAlpha.DOFade(0.6f, 0.7f); //背景黑底
    }
}
