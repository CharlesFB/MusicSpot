using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSpot : MonoBehaviour
{
    private float timer;
    private bool isClicked;
    private GameObject cam;
    private Animator ani, effectAnim;

    public bool isPaused;
    public bool isOkForTouch;
    public float touchTimeOn = 0.84f;
    public float touchTimeOff = 1.56f;
    public float destroyTime = 2.1f;
    public float perfectScore = 360;


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        isOkForTouch = false;
        isClicked = false;
        isPaused = false;
        cam = GameObject.Find("Main Camera");
        effectAnim = GetComponentsInChildren<Animator>()[1];
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime;
        }
        
        if (timer >= destroyTime)
        {
            endAnim();
        }
        else if (timer >= touchTimeOff)
        {
            noMoreTouch();
        }
        else if (timer >= touchTimeOn)
        {
            isOkForTouch = true;
        }
    }

    // 鼠标与物体碰撞时，musicspot已经添加碰撞
    public void DoTouch()
    {
        
            isClicked = true;
            ani.SetBool("GoodClick", true);
            effectAnim.SetInteger("Grade", 0);
            /*
             * 计分、结算连击数
             */
            cam.SendMessage("ComboUp");
            cam.SendMessage("ScoreUp", perfectScore);
            // 播放声音
            PlaySound();

            isOkForTouch = false;  //禁止二次点击，并延迟关闭动画
            Invoke("endAnim", 0.4f);
        
    }

    private void noMoreTouch()
    {
        isOkForTouch = false;
        //  MISS信号
        if (!isClicked)
        {
            effectAnim.SetInteger("Grade", 3);
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
            //PlayOneShot(GetComponent<AudioSource>().clip);
    }
}
