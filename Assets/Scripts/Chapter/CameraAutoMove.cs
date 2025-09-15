using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAutoMove : MonoBehaviour
{
    private int lastChapter;

    // Start is called before the first frame update
    void Start()
    {
        lastChapter = VarSingleton.instance.chapterNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastChapter != VarSingleton.instance.chapterNum)
        {
            if (lastChapter == -1)
            {
                lastChapter = VarSingleton.instance.chapterNum;
                CameraMove(lastChapter);
            }
            else
                lastChapter = VarSingleton.instance.chapterNum;
        }
        
    }

    public void CameraMove(int chapterNum)
    {
        transform.DOMove(new Vector3(VarSingleton.instance.chapterGap * chapterNum, 0, -10), 0.6f);
    }
}
