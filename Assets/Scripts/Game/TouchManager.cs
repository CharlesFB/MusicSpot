using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    private int maxCount = 10; //最大同时点触接受量
    private GameObject plane;

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        plane = GameObject.Find("Plane");
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) // 没有暂停
        {
            int count = Input.touchCount;  //获取点击数
            if (count > maxCount)
                count = maxCount;
            if (count > 0)
            {
                bool isChecked;
                for (int i = 0; i < count; i++)
                {
                    isChecked = false; //当前触摸事件是否已经被触发
                    Touch touch = Input.GetTouch(i);
                    //GameObject.Find("Canvas/Scores").GetComponent<Text>().text = touch.position.ToString();

                    Vector2 screenPos = touch.position; //获取该点击的屏幕坐标
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);  //屏幕坐标转换成世界坐标
                    Vector3 planeLocalPos = plane.transform.InverseTransformPoint(worldPos);  //世界坐标转换为相对于plane物体的相对坐标

                    //不论什么阶段，都处理touchspot
                    TouchSpot[] touchSpots = plane.GetComponentsInChildren<TouchSpot>();

                    foreach (TouchSpot spot in touchSpots)
                    {
                        GameObject obj = spot.gameObject;
                        Vector2 pos = obj.GetComponent<BoxCollider2D>().ClosestPoint(planeLocalPos); //检测触点是否在碰撞体积内
                        if (spot.isOkForTouch && Vector2.Distance(planeLocalPos, pos) <= 0.02f)
                        {
                            spot.isOkForTouch = false; // 禁二次点击
                            spot.DoTouch();
                            isChecked = true;
                            break;
                        }
                    }

                    if (isChecked) // 一个触摸事件只对应一个音符
                        continue;

                    //不论什么阶段，都处理pressspot
                    PressSpot[] pressSpots = plane.GetComponentsInChildren<PressSpot>();
                    foreach (PressSpot spot in pressSpots)
                    {
                        GameObject obj = spot.gameObject;
                        Vector2 pos = obj.GetComponent<BoxCollider2D>().ClosestPoint(planeLocalPos); //检测触点是否在碰撞体积内
                        if (spot.isOkForTouch && Vector2.Distance(planeLocalPos, pos) <= 0.02f && !spot.isPressing) //未进入按压状态，并且在允许触发的时刻成功监测到按压
                        {
                            spot.isOkForTouch = false; // 禁二次点击
                            spot.DoTouch();
                            isChecked = true;
                            break;
                        }
                        else if (Vector2.Distance(planeLocalPos, pos) > 0.02f && spot.isPressing) //在按压状态下突然未监测到手指，表示手指已移开，结算得分和连击
                        {
                            spot.DoScored();
                        }
                    }

                    if (isChecked)
                        continue;

                    if (touch.phase == TouchPhase.Began)//只有点触初始阶段处理singlespot
                    {
                        SingleSpot[] singleSpots = plane.GetComponentsInChildren<SingleSpot>();
                        foreach (SingleSpot spot in singleSpots)
                        {
                            GameObject obj = spot.gameObject;
                            Vector2 pos = obj.GetComponent<CircleCollider2D>().ClosestPoint(planeLocalPos); //检测触点是否在碰撞体积内
                            if (spot.isOkForTouch && Vector2.Distance(planeLocalPos, pos) <= 0.02f)
                            {
                                spot.isOkForTouch = false;
                                spot.DoTouch();
                                isChecked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
