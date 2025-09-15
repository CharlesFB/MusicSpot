using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpotNew : Spot
{
    public InfoManager speedInfo;
    public ResultManager resultManager;
    public EffectController effectController;

    private Animator anim;
    private float timer = 0; //计时器，用于判定点击事件

    private bool doneTouch = true;
    private bool doneOK = true;

    private void Awake()
    {
        init();
        animSpeed *= speedInfo.speedFec;
        SetAnimSpeed(animSpeed);
    }

    private void Update()
    {
        if (!GameInfoSingleton.instance.isPaused)
            timer += Time.deltaTime;

        if (timer >= deltaGap(5))   //结束时间
        {
            Miss();
            endAnim();
        }
        else if (timer >= deltaGap(2) + 0.1f)  //注释这一段==============test
        {
            if (doneTouch)
            {
                DoTouch();
                doneTouch = false;
            }
        }
        else if (timer >= deltaGap(0))   //允许点击时间
        {
            if (doneOK)
            {
                isOkForTouch = true;
                doneOK = false;
            }
        }
    }

    public void DoTouch() //执行点击事件
    {
        isOkForTouch = false; //点击后不允许再点击
        if (timer >= deltaGap(2) && timer < deltaGap(3))
            PerfectClick();
        else if (timer >= deltaGap(1) && timer < deltaGap(4))
            GreatClick();
        else if (timer >= deltaGap(0) && timer < deltaGap(5))
            BadClick();
    }

    private void init()
    {
        anim = GetComponent<Animator>();
        isOkForTouch = false;
        type = 0; //告诉父级这个继承来自与单击音符
        speedInfo = GameObject.Find("InfoManager").GetComponent<InfoManager>();
        resultManager = GameObject.Find("ResultManager").GetComponent<ResultManager>();
        effectController = GameObject.Find("EffectController").GetComponent<EffectController>();
    }

    public void SetAnimSpeed(float s)
    {
        anim.speed = s;
    }

    private void PerfectClick()
    {
        anim.SetInteger("Click", 1);
        effectController.SetEffect(transform.position,0);
        PlaySound();
        
        resultManager.ComboUp();
        resultManager.PerfectUp();
        resultManager.ScoreUp(GameInfoSingleton.instance.singlePerfectScore);
    }

    private void GreatClick()
    {
        anim.SetInteger("Click", 1);
        effectController.SetEffect(transform.position, 1);
        PlaySound();
        
        resultManager.ComboUp();
        resultManager.GreatUp();
        resultManager.ScoreUp((int)(GameInfoSingleton.instance.singlePerfectScore * 0.6f));
    }

    private void BadClick()
    {
        anim.SetInteger("Click", 2);
        effectController.SetEffect(transform.position, 2);
        
        resultManager.ComboReset();
        resultManager.BadUp();
        resultManager.ScoreUp((int)(GameInfoSingleton.instance.singlePerfectScore * 0.14f));
    }

    private void Miss()
    {
        if (isOkForTouch)
        {
            isOkForTouch = false;
            effectController.SetEffect(transform.position, 3);
            resultManager.ComboReset();
            resultManager.MissUp();
            //PerfectClick();
        }
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
