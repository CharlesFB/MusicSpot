using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PressSpotNew : Spot
{
    public GameObject bar;
    public GameObject back;
    public GameObject button;
    public InfoManager speedInfo;
    public ResultManager resultManager;
    public EffectController effectController;

    public int dir; //0：向上， 1：向下
    /* spd: 用于动态调整变速
     * （以100bpm 1拍曲速为基准，初始基准速度为0.6秒/节）
     * 0：快1倍
     * 1：正常
     * 2：慢1番
     * 3：慢2番
     * 
     * 说人话就是分别在0.5/1/2/4个小节内完成长条读条动画，用于动态调整变速问题
     */
    public int spd; 
    public bool isPressing;  //是否处于按压状态

    private Animator buttonAnim;

    private Tweener backLoading;  //用于保存读条运动动画，用户在松手的时候会主动停止动画
    private float loadTime = 0.6f; //动画应加载的时间
    private float desSize; //目标back应达到的尺寸，用于判断最终得分
    private float timer = 0;
    private bool loadOn = true;
    private bool disappearOn = true;

    private float gameWidth, gameHeight;  //游戏屏幕的最大宽高
    private static float GAME_SCREEN_VERTICAL_FEC = 0.64f;
    private static float GAME_SCREEN_HORIZ_FEC = 0.9f;

    private bool donePress = true, doneRelease = true;

    private void Start()
    {
        init();
        animSpeed *= speedInfo.speedFec;
        SetAnimSpeed(animSpeed);
        bar.GetComponent<Image>().DOFade(1, 0.8f);
    }

    private void Update()
    {
        if (!GameInfoSingleton.instance.isPaused)
        {
            timer += Time.deltaTime;
        }
        if (timer > deltaGap(2) + loadTime + 1f)
        {
            endAnim();
        }
        else if (timer > deltaGap(2) + loadTime)
        {
            if (doneRelease)
            {
                doneRelease = false;
                DoRelease();  //注释这一行========================test
                DoDisappear();
            }
        }
        else if (timer > deltaGap(2) + loadTime/3)
        {
            Miss();
        }
        else if (timer >= deltaGap(2))
        {
            isOkForTouch = true;
            BackLoadingAnim();
            if (donePress)
            {
                DoPress();  //注释这一行===========================test
                donePress = false;
            }
        }
    }

    private void init() //需要在实例化的时候分配type
    {
        isPressing = false; //开始处于未按压
        buttonAnim = button.GetComponent<Animator>();
        isOkForTouch = false;
        speedInfo = GameObject.Find("InfoManager").GetComponent<InfoManager>();
        resultManager = GameObject.Find("ResultManager").GetComponent<ResultManager>();
        effectController = GameObject.Find("EffectController").GetComponent<EffectController>();

        gameWidth = Screen.width * GAME_SCREEN_HORIZ_FEC / 2;
        gameHeight = Screen.height * GAME_SCREEN_VERTICAL_FEC / 2;

        BarInit(); //根据type显示bar的长度
        DirInit(); //根据dir 调整朝向
        SpdInit(); //根据spd 调整动画播放时间
    }

    private void SpdInit()
    {
        loadTime = speedInfo.secPerBeat * 2;
        float rate = 1;
        switch (spd)
        {
            case 0:
                rate *= 0.5f;
                break;
            case 1:
                rate *= 1;
                break;
            case 2:
                rate *= 2;
                break;
            case 3:
                rate *= 4;
                break;
        }
        loadTime *= rate;
    }

    private void BarInit()
    {
        RectTransform barRect = bar.GetComponent<RectTransform>();
        RectTransform backRect = back.GetComponent<RectTransform>();
        switch (this.type)
        {
            case 2: //   1/4屏距
                desSize = backRect.sizeDelta.y + 1 / 4 * gameHeight;
                barRect.sizeDelta = new Vector2(barRect.sizeDelta.x,barRect.sizeDelta.y + 1 / 4 * gameHeight);
                break;
            case 3: //   1/2屏距
                desSize = backRect.sizeDelta.y + 1 / 2 * gameHeight;
                barRect.sizeDelta = new Vector2(barRect.sizeDelta.x, barRect.sizeDelta.y + 1 / 2 * gameHeight);
                break;
            case 4: //     1屏距
                desSize = backRect.sizeDelta.y + gameHeight;
                barRect.sizeDelta = new Vector2(barRect.sizeDelta.x, barRect.sizeDelta.y + gameHeight);
                break;
        }
    }

    private void DirInit()
    {
        if (dir == 1)
        {
            GetComponent<RectTransform>().Rotate(0, 0, 180);
        }
    }

    public void DoPress()  //按压状态函数
    {
        isPressing = true;
        ButtonDoScaleAnim();
        buttonAnim.SetBool("Pressing", true);

        Image backImage = back.GetComponent<Image>();
        backImage.DOFade(0.7f, 0.05f).SetId<Tween>("game"); //按压状态下读条透明度恢复
    }

    public void DoRelease()  //松手状态函数
    {
        isPressing = false;
        if (backLoading.IsPlaying())
            backLoading.Pause();
        button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //按钮大小恢复
        buttonAnim.SetBool("Pressing", false);

        RectTransform backRect = back.GetComponent<RectTransform>();
        Vector3 effectPos = backRect.position;
        if (dir == 1)
        {
            effectPos.y -= backRect.sizeDelta.y;
        }
        else
        {
            effectPos.y += backRect.sizeDelta.y;
        }

        float desPercentage = backRect.sizeDelta.y / desSize;
        if (desPercentage >= 0.85f)   //Perfect区域：读条到应当达到长度的85%以上
        {
            PerfectClick(effectPos);
        }
        else if (desPercentage >= 0.7f) //Great区域：读条应达到长度的85%以下，70%以上
        {
            GreatClick(effectPos);
        }
        else  //Bad区域：读条长度不到70%
        {
            BadClick(effectPos);
        }
    }

    public void DoDisappear()
    {
        if (disappearOn)
        {
            disappearOn = false;
            Image barImage = bar.GetComponent<Image>();
            Image backImage = back.GetComponent<Image>();
            Image buttonImage = button.GetComponent<Image>();
            barImage.DOFade(0, 0.17f).SetId<Tween>("game");
            backImage.DOFade(0, 0.17f).SetId<Tween>("game");
            buttonAnim.SetBool("Disappear", true);
        }
    }

    public void SetAnimSpeed(float s)
    {
        buttonAnim.speed *= s;
    }

    private void BackLoadingAnim()
    {
        if (loadOn)
        {
            loadOn = false;
            RectTransform backRect = back.GetComponent<RectTransform>();
            RectTransform barRect = bar.GetComponent<RectTransform>();
            backLoading = backRect.DOSizeDelta(new Vector2(backRect.sizeDelta.x, barRect.sizeDelta.y), loadTime).SetEase(Ease.Linear).SetId("game");
        }
    }

    private void ButtonDoScaleAnim() //按住使button部分scale增加到1.25倍
    {
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.localScale = new Vector3(1.25f, 1.25f, 1.25f);
    }

    private void PerfectClick(Vector3 effectPos)
    {
        effectController.SetEffect(effectPos, 0);
        PlaySound();
        
        resultManager.ComboUp();
        resultManager.PerfectUp();
        resultManager.ScoreUp(GameInfoSingleton.instance.pressPerfectScore);
    }

    private void GreatClick(Vector3 effectPos)
    {
        effectController.SetEffect(effectPos, 1);
        PlaySound();
        
        resultManager.ComboUp();
        resultManager.GreatUp();
        resultManager.ScoreUp((int)(GameInfoSingleton.instance.pressPerfectScore * 0.6f));
    }

    private void BadClick(Vector3 effectPos)
    {
        effectController.SetEffect(effectPos, 2);
        
        resultManager.ComboReset();
        resultManager.BadUp();
        resultManager.ScoreUp((int)(GameInfoSingleton.instance.pressPerfectScore * 0.14f));
    }

    private void Miss()
    {
        Image barImage = bar.GetComponent<Image>();
        Image buttonImage = button.GetComponent<Image>();
        if (isOkForTouch && !isPressing)
        {
            isOkForTouch = false;
            barImage.DOFade(0.4f, 0.05f).SetId("game");
            buttonImage.DOFade(0.4f, 0.05f).SetId("game");
            resultManager.ComboReset();
            resultManager.MissUp();
            //PerfectClick();
        }
    }

    private void PlaySound() //成功触发音效
    {

    }

    private float deltaGap(int i) //range (0,5)
    {
        return GameInfoSingleton.instance.gaps[i] / speedInfo.speedFec;
    }
}
