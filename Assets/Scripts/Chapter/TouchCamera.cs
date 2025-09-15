using UnityEngine;
public class TouchCamera : MonoBehaviour
{
    public bool canPan = true;

    //around center
    public Transform target;
    private Vector3 targetPan;
    private Vector3 currentPan;
    private float v; //移动初速度
    private float a = 30; // 加速度

    private bool getCurrentDA = true;

    //Damper(阻尼) for move
    [Range(0, 10)]
    private float damper = 2;

    void Start()
    {
        GameObject camTargetObj = GameObject.Find("Main Camera");
        if (camTargetObj == null)
            camTargetObj = new GameObject("Main Camera");
        target = camTargetObj.transform;
        v = 0;
        currentPan = targetPan = transform.position;
    }

    void Update()
    {
        if (VarSingleton.instance.chapterNum == -1) //在没有选中任何一个
        {
            if (Input.touchCount <= 0)
            {
                getCurrentDA = true;

                if (v != 0)
                {
                    //没检测到触屏的时候根据速度v进行减速移动
                    float t = Time.deltaTime;
                    if ((v + a * t) * v > 0) // 没有减速超过0，即减速前后v的方向不变（同号）
                    {

                        targetPan.x -= v * t; // 位移公式\
                        if (targetPan.x < 0)
                        {
                            targetPan.x = 0;
                            v = 0;
                        }
                        else if (targetPan.x > (VarSingleton.instance.chapterCount-1) * VarSingleton.instance.chapterGap)
                        {
                            targetPan.x = (VarSingleton.instance.chapterCount - 1) * VarSingleton.instance.chapterGap;
                            v = 0;
                        }
                        else
                            v += a * t; // 速度变化公式

                        transform.position = targetPan;
                    }
                    else
                        v = 0;
                }


                return;
            }
            else
            {
                v = 0; // 初速度清零
                       //确保获取最新的Camera状态
                if (getCurrentDA)
                {
                    getCurrentDA = false;
                    currentPan = targetPan = transform.position;
                }
            }

            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //平移
                    if (canPan)
                    {
                        //v = touch.deltaPosition.x / touch.deltaTime;
                        targetPan.x -= touch.deltaPosition.x / 100;
                        currentPan = Vector3.Lerp(currentPan, targetPan, (damper) * Time.deltaTime);
                        if (targetPan.x < 0)
                        {
                            targetPan.x = 0;
                        }
                        else if (targetPan.x > (VarSingleton.instance.chapterCount - 1) * VarSingleton.instance.chapterGap)
                        {
                            targetPan.x = (VarSingleton.instance.chapterCount - 1) * VarSingleton.instance.chapterGap;
                        }
                        transform.position = targetPan;
                    }

                    //v = Mathf.Clamp(touch.deltaPosition.x / touch.deltaTime, -30, 30);//获取手指离开时的瞬时速度作为初速度，限制速度范围
                    //if (v != 0)
                    //    a = -1 * Mathf.Abs(a) * (v / Mathf.Abs(v)); //调整加速度的方向与v相反

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    v = Mathf.Clamp(touch.deltaPosition.x / touch.deltaTime, -30, 30); //获取手指离开时的瞬时速度作为初速度
                    if (v != 0) //很关键！！！！！！
                        a = -1 * Mathf.Abs(a) * (v / Mathf.Abs(v)); //调整加速度的方向与v相反
                                                                    //Debug.Log(v);
                                                                    //Debug.Log(a);
                }
            }
        }

    }
}
