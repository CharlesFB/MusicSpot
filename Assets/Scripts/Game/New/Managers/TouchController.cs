using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public int maxCount = 10; //最大同时点触接受量
    public GameObject spotsPanel;

    void Update()
    {
        if (!GameInfoSingleton.instance.isPaused) // 没有暂停
        {
            int count = Input.touchCount;  //获取点击数
            if (count > maxCount)
                count = maxCount;  //超过最大点击量则置为最大
            if (count > 0)     //如果有触屏事件
            {
                //Debug.Log("you dian ji shi jian");
                Spot[] spots = spotsPanel.GetComponentsInChildren<Spot>(); //获取谱面元素面板下全部子物体（获取当前场上全部的谱面元素）
                List<Spot> spotsOkForTouch = new List<Spot>(); //用来存放按生成顺序先后排列的，可以点击的spots
                //Debug.Log("==================upward=============");
                for (int j = spots.Length - 1; j >= 0; j--) //倒序为生成顺序
                {
                    if (spots[j].isOkForTouch == true)
                    {
                        spotsOkForTouch.Add(spots[j]);
                        //Debug.Log(spots[j]);
                    }
                }
                

                for (int i = 0; i < count; i++)
                {
                    Touch touch = Input.GetTouch(i); //获取第i+1根手指的点击事件

                    Vector2 screenPos = touch.position; //获取该点击的屏幕坐标
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);  //屏幕坐标转换成世界坐标
                    Vector3 planeLocalPos = spotsPanel.transform.InverseTransformPoint(worldPos);  //世界坐标转换为相对于spotsPanel物体的相对坐标

                    //Debug.Log("jin ru for");

                    bool isBond = false; //判断此次点击是否被绑定过（每次点击只能绑定一个谱面元素）
                    for (int j = 0; j < spotsOkForTouch.Count; j++) //遍历所有可点击、按顺序排列的谱面元素，以此判断用户点击是否在碰撞体积范围内、点击的是哪种音符
                    {
                        switch (spotsOkForTouch[j].type)
                        {
                            case 0: //只在点击初始阶段处理单击音符 SingleSpot
                                if (touch.phase == TouchPhase.Began)
                                {
                                    //Debug.Log("====11==1=1=1=1=1=1=1=1==1=1===");
                                    SingleSpotNew spot = (SingleSpotNew)spotsOkForTouch[j]; 
                                    Vector2 pos = spot.gameObject.GetComponent<CircleCollider2D>().ClosestPoint(worldPos);
                                    
                                    if (Vector2.Distance(worldPos, pos) <= 0.02f)
                                    {
                                        spot.DoTouch();
                                        isBond = true;
                                    }
                                }
                                break;
                            case 1: //什么阶段都会处理触发音符 TouchSpot
                                {
                                    TouchSpotNew spot = (TouchSpotNew)spotsOkForTouch[j];
                                    Vector2 pos = spot.gameObject.GetComponent<PolygonCollider2D>().ClosestPoint(worldPos);
                                    if (Vector2.Distance(worldPos, pos) <= 0.02f)
                                    {
                                        spot.DoTouch();
                                        isBond = true;
                                    }
                                }
                                break;
                            case 2: //什么阶段都处理蓄力音符 PressSpot
                            case 3:
                            case 4:
                                {
                                    PressSpotNew spot = (PressSpotNew)spotsOkForTouch[j];
                                    Vector2 pos = spot.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(worldPos);
                                    if (Vector2.Distance(worldPos, pos) <= 0.02f && !spot.isPressing)
                                    {
                                        spot.DoPress();
                                        isBond = true;
                                    }
                                }
                                break;
                        }
                        if (isBond) //一旦被绑定过直接退出循环
                            break;
                    }
                }

                //单独检测在按压状态下的 PressSpot，判断是否需要进入release阶段
                for (int i = 0; i < spotsOkForTouch.Count; i++)
                {
                    switch (spotsOkForTouch[i].type)
                    {
                        case 2:
                        case 3:
                        case 4:
                            PressSpotNew spot = spotsOkForTouch[i].gameObject.GetComponent<PressSpotNew>();
                            if (spot.isPressing) //按压状态
                            {
                                bool isOkForRelease = true;
                                for (int j = 0; j < count; j++)
                                {
                                    Touch touch = Input.GetTouch(j);
                                    Vector2 screenPos = touch.position; //获取该点击的屏幕坐标
                                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);  //屏幕坐标转换成世界坐标
                                    Vector3 planeLocalPos = spotsPanel.transform.InverseTransformPoint(worldPos);  //世界坐标转换为相对于spotsPanel物体的相对坐标

                                    Vector2 pos = spot.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(worldPos);
                                    if (Vector2.Distance(worldPos, pos) <= 0.02f)
                                    {
                                        isOkForRelease = false;
                                        break;
                                    }    
                                }
                                if (isOkForRelease)
                                {
                                    spot.DoRelease();
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
