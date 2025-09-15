using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private Animator ani;
    private bool isOkForCount;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        setFalse();
        isOkForCount = false;
        Invoke("Count", 0.5f); //进入场景后半秒等待播放倒计时
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isOkForCount)
        {
            ani.SetBool("Count", true);
            isOkForCount = false;
            Invoke("setFalse", 0.1f);
        }
    }

    public void Count()
    {
        isOkForCount = true;
    }

    private void setFalse()
    {
        ani.SetBool("Count", false);
    }
}
