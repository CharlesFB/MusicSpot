using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Image topBlur;
    public Image blackAlpha;
    public MusicManager musicManager;
    public RectTransform pauseBox;
    public CountDownNew countdown;
    public Button pauseButton;
    public GameObject spotsPanel;
    public GameObject panel;
    public AudioSource pauseAudio;
    public ScenesBlackFade black;

    private void Start()
    {
        pauseButton.interactable = false;
        Invoke("ButtonReenable", 4f);

        //Invoke("PauseButtonClick", 6f);
    }

    public void PauseButtonClick()
    {
        //Debug.Log("===========================Here========================");

        pauseBox.position = new Vector3(0, 0, 0);
        topBlur.DOFade(1, 0.1f);
        blackAlpha.DOFade(0.4f, 0.1f);

        GameInfoSingleton.instance.isPaused = true; //暂停所有音符、Manager判定动作
        musicManager.AudioPause(); //暂停音乐播放，同时顺便暂停了音符生成、判定线移动等
        Animator[] animators = spotsPanel.GetComponentsInChildren<Animator>();
        foreach (Animator ani in animators)
        {
            if (ani != null)
            {
                ani.speed = 0; //暂停所有正在进行的帧动画
            }
        }
        DOTween.Pause("game");
        pauseButton.interactable = false; // 暂停键禁用
        // 播放声音
        pauseAudio.Play();
    }

    public void ResumeClicked()
    {
        countdown.Count();
        pauseBox.position = new Vector3(0, Screen.height, 0);
        Invoke("Resume", 3);
    }

    private void Resume()
    {
        topBlur.DOFade(0, 0.1f);
        blackAlpha.DOFade(0, 0.1f); //移去遮挡

        GameInfoSingleton.instance.isPaused = false; //启用
        musicManager.AudioPlay(); //播放音乐
        Animator[] animators = spotsPanel.GetComponentsInChildren<Animator>();
        foreach (Animator ani in animators)
        {
            if (ani != null)
            {
                ani.speed = ani.gameObject.GetComponent<Spot>().animSpeed; //恢复所有帧动画
            }
        }
        DOTween.Play("game");
        pauseButton.interactable = true; // 暂停键启用
    }

    public void ExitThis()
    {
        black.blackFadeIn();
        GameInfoSingleton.instance.isPaused = false; //启用
        Invoke("LoadChapter", 0.5f);
    }

    private void LoadChapter()
    {
        SceneManager.LoadScene("ChapterPage");
    }

    private void ButtonReenable()
    {
        pauseButton.interactable = true;
    }

    public void Restart()
    {
        GameInfoSingleton.instance.isPaused = false; //启用
        SceneManager.LoadScene("Game");
    }
}
