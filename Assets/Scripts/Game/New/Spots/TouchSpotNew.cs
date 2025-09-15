using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSpotNew : Spot
{
    public InfoManager speedInfo;
    public ResultManager resultManager;
    public EffectController effectController;

    private Animator anim;
    private float timer = 0;

    private bool doneTouch = true;

    private void Awake()
    {
        init();
        animSpeed *= speedInfo.speedFec;
        SetAnimSpeed(animSpeed);
    }

    private void init()
    {
        anim = GetComponent<Animator>();
        isOkForTouch = false;
        type = 1; //告诉父级这个继承来自与触发音符
        speedInfo = GameObject.Find("InfoManager").GetComponent<InfoManager>();
        resultManager = GameObject.Find("ResultManager").GetComponent<ResultManager>();
        effectController = GameObject.Find("EffectController").GetComponent<EffectController>();
    }

    private void Update()
    {
        if (!GameInfoSingleton.instance.isPaused)
        {
            timer += Time.deltaTime;
        }
        if (timer >= deltaGap(3))   //结束时间
        {
            Miss();
            endAnim();
        }
        else if (timer >= deltaGap(2) + 0.1f)
        {
            if (doneTouch)
            {
                doneTouch = false;
                DoTouch(); //直接命中=========================注释这一句test
            }
        }
        else if (timer >= deltaGap(2))   //允许点击时间
        {
            isOkForTouch = true;
        }
    }

    public void DoTouch()
    {
        isOkForTouch = false; //点击后不允许再点击
        anim.SetBool("Touch", true);
        effectController.SetEffect(transform.position, 0);
        PlaySound();
        
        resultManager.ComboUp();
        resultManager.PerfectUp();
        resultManager.ScoreUp(GameInfoSingleton.instance.touchPerfectScore);
    }

    private void Miss()
    {
        if (isOkForTouch) 
        {
            isOkForTouch = false;
            effectController.SetEffect(transform.position, 3);
            resultManager.ComboReset();
            resultManager.MissUp();
            { //Perfect
                //anim.SetBool("Touch", true);
                //effectController.SetEffect(transform.position, 0);
                //PlaySound();

                //resultManager.ComboUp();
                //resultManager.PerfectUp();
                //resultManager.ScoreUp(GameInfoSingleton.instance.touchPerfectScore);
            }
        }
    }

    public void SetAnimSpeed(float s)
    {
        anim.speed = s;
    }

    private void PlaySound()
    {
        GameObject.Find("MusicManager").GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
    }

    private float deltaGap(int i) //range (0,5)
    {
        return GameInfoSingleton.instance.gaps[i] / speedInfo.speedFec;
    }
}
