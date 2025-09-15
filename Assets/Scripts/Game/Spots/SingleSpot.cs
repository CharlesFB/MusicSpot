using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpot : MonoBehaviour
{
    public float timeBegin = 0; // 需要在动态生成的时候修改
    public bool isPaused;
    public bool isOkForTouch;
    public float touchTimeOn = 0.73f;
    public float touchTimeOff = 1.5f;
    public float destroyTime = 2.1f;
    public float greatScaleLeft = 0.9f;
    public float greatScaleRight = 1.41f;
    public float perfectScaleLeft = 1.05f;
    public float perfectScaleRight = 1.38f;
    public int perfectScore = 360;

    private bool isClicked;
    private GameObject cam;
    private AudioSource aud;
    private Animator ani, effectAni;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        isOkForTouch = false;
        isClicked = false;
        isPaused = false;

        cam = GameObject.Find("Main Camera");
        aud = cam.GetComponent<AudioSource>();
        effectAni = GetComponentsInChildren<Animator>()[1];

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)  //供暂停使用,暂停动画和行为
            timer += Time.deltaTime;
        
        if (timer >= destroyTime)   // 1.39s + Disappear 0.28s + 0.23s(wait) | 0.28 = 00:27 / 01:00 / 1.62
        {
            endAnim();
        }
        else if (timer >= touchTimeOff)   // 1.39s + 0.2s(wait)
        {
            noMoreTouch();
        }
        else if (timer >= touchTimeOn)   // Begin 0.33s + 0.4s(wait)
        {
            isOkForTouch = true;
        }

    }

    // 鼠标与物体碰撞时。（musicspot已经添加碰撞）
    public void DoTouch()
    {
            isClicked = true;
            isOkForTouch = false;
            /*
             * 处理点击该音符后的事件
             */
            float deltaTime = aud.time - timeBegin; // 获得差值时间
            //Debug.Log(deltaTime);
            if (deltaTime < greatScaleLeft || deltaTime >= greatScaleRight)
            {
                // BAD信号
                ani.SetBool("BadClick", true);
                effectAni.SetInteger("Grade", 2);
                cam.SendMessage("ScoreUp", perfectScore/6);
                cam.SendMessage("ComboReset");
                /*
                 * 计分、结算连击数
                 */

            }
            else if (deltaTime < perfectScaleLeft || deltaTime >= perfectScaleRight)
            {
                // GREAT信号
                ani.SetBool("GoodClick", true);
                effectAni.SetInteger("Grade", 1);
                /*
                 * 计分、结算连击数
                 */
                cam.SendMessage("ComboUp");
                cam.SendMessage("ScoreUp",perfectScore*3/5);
                PlaySound();
            }
            else
            {//期望1.18s
                // PERFECT信号
                ani.SetBool("GoodClick", true);
                effectAni.SetInteger("Grade", 0);
                /*
                 * 计分、结算连击数
                 */
                cam.SendMessage("ComboUp");
                cam.SendMessage("ScoreUp", perfectScore);
                PlaySound();
            }

            isOkForTouch = false;
            Invoke("endAnim", 0.3f);
        
    }

    private void noMoreTouch()
    {
        isOkForTouch = false;
        //  MISS信号
        if (!isClicked)
        {
            effectAni.SetInteger("Grade", 3);
            /*
             * 结算连击数
             */
            cam.SendMessage("ComboReset");
        }
    }

    private void endAnim()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
