using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameObject cam;
    public GameObject pauseBox, pauseBackScene, pauseBlur, countdown;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        Invoke("Reenabled", 4f);
    }

    private void Reenabled()
    {
        GetComponent<Button>().enabled = true; // 暂停键启用
    }

    public void ButtonClicked()
    {
        pauseBox.GetComponent<RectTransform>().position = new Vector3(0, 0);
        pauseBackScene.GetComponent<Transform>().position = new Vector3(0, 0);
        pauseBlur.GetComponent<Transform>().position = new Vector3(0, 0);
        //Time.timeScale = 0;

        GameObject spotsPlane = GameObject.Find("Plane");
        GameObject linePlane = GameObject.Find("Plane(Line)");
        GameObject pause = GameObject.Find("Canvas/Pause");
        GameObject touchManager = GameObject.Find("TouchManager");

        Animator[] spotsAnimators = spotsPlane.GetComponentsInChildren<Animator>();
        Animator[] linesAnimators = linePlane.GetComponentsInChildren<Animator>();
        SingleSpot[] scriptsSingle = spotsPlane.GetComponentsInChildren<SingleSpot>();
        TouchSpot[] scriptsTouch = spotsPlane.GetComponentsInChildren<TouchSpot>();
        PressSpot[] scriptsPress = spotsPlane.GetComponentsInChildren<PressSpot>();

        cam.GetComponent<CameraBehavior>().audioPause(); //音乐暂停
        touchManager.GetComponent<TouchManager>().isPaused = true; // 停止点触检测
        
        foreach (SingleSpot script in scriptsSingle)  //音符禁止点击、停止行为
        {
            if (script != null)
            {
                script.isOkForTouch = false;
                script.isPaused = true;
            }
        }
        foreach (TouchSpot script in scriptsTouch)
        {
            if (script != null)
            {
                script.isOkForTouch = false;
                script.isPaused = true;
            }
        }
        foreach(PressSpot script in scriptsPress)
        {
            if (script != null)
            {
                script.isOkForTouch = false;
                script.isPaused = true;
            }
        }
        foreach (Animator aniSpots in spotsAnimators) //音符停止动画
        {
            if (aniSpots != null)
            {
                aniSpots.speed = 0;
                
            }
        }
        foreach (Animator aniLines in linesAnimators) //线停止动画
        {
            if (aniLines != null)
            {
                aniLines.speed = 0;
            }
        }
        pause.GetComponent<Button>().enabled = false; // 暂停键禁用
        // 播放声音
        GetComponent<AudioSource>().Play();
    }

    public void ResumeClicked()
    {
        Animator ani = countdown.GetComponent<Animator>();
        ani.SetBool("Count", true); //播放倒计时

        pauseBox.GetComponent<RectTransform>().position = new Vector3(0, 1392);
        pauseBackScene.GetComponent<Transform>().position = new Vector3(0, 6.23f);
        pauseBlur.GetComponent<Transform>().position = new Vector3(0, 6.23f);
        Invoke("Resume", 3);
    }

    private void Resume()
    {
        //Time.timeScale = 1;
        Animator ani = countdown.GetComponent<Animator>();
        ani.SetBool("Count", false); //倒计时idle

        GameObject spotsPlane = GameObject.Find("Plane");
        GameObject linePlane = GameObject.Find("Plane(Line)");
        GameObject pause = GameObject.Find("Canvas/Pause");
        GameObject touchManager = GameObject.Find("TouchManager");

        Animator[] spotsAnimators = spotsPlane.GetComponentsInChildren<Animator>();
        Animator[] linesAnimators = linePlane.GetComponentsInChildren<Animator>();
        SingleSpot[] scriptsSingle = spotsPlane.GetComponentsInChildren<SingleSpot>();
        TouchSpot[] scriptsTouch = spotsPlane.GetComponentsInChildren<TouchSpot>();
        PressSpot[] scriptsPress = spotsPlane.GetComponentsInChildren<PressSpot>();

        touchManager.GetComponent<TouchManager>().isPaused = false; // 启用点触检测
        foreach (SingleSpot script in scriptsSingle)  //音符点击启用、行为开启
        {
            if (script != null)
            {
                script.isOkForTouch = true;
                script.isPaused = false;
            }
        }
        foreach (TouchSpot script in scriptsTouch)
        {
            if (script != null)
            {
                script.isOkForTouch = true;
                script.isPaused = false;
            }
        }
        foreach (PressSpot script in scriptsPress)
        {
            if (script != null)
            {
                script.isOkForTouch = true;
                script.isPaused = false;
            }
        }
        foreach (Animator aniSpots in spotsAnimators) //音符动画播放
        {
            if (aniSpots != null)
            {
                aniSpots.speed = 1;
            }
        }
        foreach (Animator aniLines in linesAnimators) //线动画播放
        {
            if (aniLines != null)
            {
                aniLines.speed = 1;
            }
        }
        pause.GetComponent<Button>().enabled = true; // 暂停键启用
        cam.GetComponent<CameraBehavior>().audioPlay(); //播放音乐
    }

    public void ExitThis()
    {
        SceneManager.LoadScene("ChapterPage");
    }

    public void Restart()
    {
        SceneManager.LoadScene(VarSingleton.instance.musics[VarSingleton.instance.musicNum].musicName
            + "_" + VarSingleton.instance.diff.ToString());
    }

}
