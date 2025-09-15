using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSpot : MonoBehaviour
{
    public float barHeight4 = 1.392f;
    public float barHeight8 = 0.696f;
    public float barHeight16 = 0.348f; //对应几分音符的读条长度
    public float barPosY4 = 8.43f;
    public float barPosY8 = 3.484f;
    public float barPosY16 = 1.25f; //对应几分音符的读条位置
    public float deltaTime4 = 0.54f;
    public float deltaTime8 = 0.27f;
    public float deltaTime16 = 0.14f; //对应几分音符的读满条时间

    public float perfectOffset = 1.18f; //期望1.18秒开始读条
    public float touchTimeOn = 1.17f;
    public float touchTimeOff = 1.31f;
    public int perfectScore = 360;

    public bool isPaused;

    public bool isOkForTouch;
    public bool isPressing;
    private bool isOverEarlier;
    private int direct = 0;
    private int length = 0;
    private Animator ani, progressAni, barAni, effectAni;
    private float fullTime = 10f; // 读条结束时间

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isPaused = false;
        isOkForTouch = false;
        isPressing = false;
        isOverEarlier = false;

        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            switch (anim.name)
            {
                case "ProgressBack":
                    progressAni = anim;
                    break;
                case "ProgressBar":
                    barAni = anim;
                    break;
                case "Grade":
                    effectAni = anim;
                    break;
                default:
                    ani = anim;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime;
        }
        
        if (timer >= fullTime && !isOverEarlier)
        {
            // 读条结束
            ani.SetBool("Disappear", true);
            progressAni.SetBool("Disappear", true);
            barAni.SetBool("Disappear", true);
            if (!isPressing)
            {
                //MISS Grade
                effectAni.SetInteger("Grade", 3);
            }
            else
            {
                //PERFECT Grade
                effectAni.SetInteger("Grade", 0);
                Camera.main.SendMessage("ComboUp");
                Camera.main.SendMessage("ScoreUp", perfectScore);
                // 播放声音
            }
            Invoke("endAnim", 0.4f); //销毁

        }
        else if (timer >= touchTimeOff && !isPressing)
        {
            noMoreTouch();
            //MISS信号
            /*
             * 结算连击数
             */
            Camera.main.SendMessage("ComboReset");
        }
        else if (timer >= perfectOffset)
        {
            // 开始读条，播放动画，但不开透明度
            if (direct == 0) // 向上
                progressAni.SetBool("Up", true);
            else //向下
                progressAni.SetBool("Down", true);
        }
        else if (timer >= touchTimeOn)
        {
            isOkForTouch = true;
        }
    }

    public void init(int dir, int len) // 实例化后必要执行，用于设置读条出现的位置
    {
        Debug.Log("================");
        Debug.Log(dir);
        Debug.Log(len);

        direct = dir;
        length = len;
        int x = 1; // 系数，控制方向
        if (dir == 1) // 反向
            x = -1;

        Transform pb = null;//获取progressBar的transform
        Transform eff = null; // 获取特效的transform
        foreach (Transform tf in GetComponentsInChildren<Transform>())
        {
            if (tf.gameObject.name == "ProgressBar")
            {
                pb = tf;
            }else if (tf.gameObject.name == "Grade")
            {
                eff = tf;
            }
        }

        switch (length)
        {
            case 0: // 4分
                fullTime = touchTimeOn + deltaTime4;
                pb.localPosition = new Vector3(0, barPosY4 * x, 0);
                pb.localScale = new Vector3(pb.localScale.x, barHeight4 * x, pb.localScale.z);
                break;
            case 1: // 8分
                fullTime = touchTimeOn + deltaTime8;
                pb.localPosition = new Vector3(0, barPosY8 * x, 0);
                pb.localScale = new Vector3(pb.localScale.x, barHeight8 * x, pb.localScale.z);
                break;
            case 2: // 16分
                fullTime = touchTimeOn + deltaTime16;
                pb.localPosition = new Vector3(0, barPosY16 * x, 0);
                pb.localScale = new Vector3(pb.localScale.x, barHeight16 * x, pb.localScale.z);
                break;
        }

        Debug.Log(fullTime);
        Debug.Log("------------------");

        eff.position = pb.position; // 设置特效的位置
    }

    public void DoTouch()
    {
            isPressing = true; // 进入按压状态
            ani.SetBool("Press", true); //播放按压动画
            BackProgressAlphaOn(); //恢复读条透明度
        
    }

    public void DoScored()
    {
        isPressing = false;
        float deltaTime = timer - touchTimeOn;
        float percentage = deltaTime / (fullTime - touchTimeOn);
        ani.SetBool("Disappear", true);
        progressAni.SetBool("Disappear", true);
        barAni.SetBool("Disappear", true); // 消失动画
        if (percentage >= 0.6f)
        {
            //PERFFECT
            effectAni.SetInteger("Grade", 0);
            Camera.main.SendMessage("ComboUp");
            Camera.main.SendMessage("ScoreUp", perfectScore);
        }
        else if (percentage >= 0.4f)
        {
            //GREAT
            effectAni.SetInteger("Grade", 1);
            /*
             * 计分、结算连击数
             */
            Camera.main.SendMessage("ComboUp");
            Camera.main.SendMessage("ScoreUp", perfectScore * 3 / 5);
        }
        else
        {
            //BAD
            effectAni.SetInteger("Grade", 2);
            Camera.main.SendMessage("ScoreUp", perfectScore / 6);
            Camera.main.SendMessage("ComboReset");
        }
        isOverEarlier = true; //标识已提前结束，不要重复播放
    }

    private void BackProgressAlphaOn()  //打开透明度
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in spriteRenderers)
        {
            if (child.gameObject.name == "ProgressBack")
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 1);
            }
        }
    }

    private void noMoreTouch()  //禁止点击并调低透明度标识
    {
        isOkForTouch = false;
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer child in spriteRenderers)
        {
            if (child.gameObject.name != "Grade")
            {
                child.color = new Color(child.color.r, child.color.g, child.color.b, 0.2f);
            }
        }
    }

    private void endAnim()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
