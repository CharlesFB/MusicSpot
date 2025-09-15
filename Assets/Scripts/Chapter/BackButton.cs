using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonOnClicked()
    {
        if (VarSingleton.instance.chapterNum != -1)
        {
            GetComponent<Image>().DOFade(0, 0.4f);
            GetComponent<Button>().enabled = false;
            VarSingleton.instance.chapters[VarSingleton.instance.chapterNum].lastPlayedId =
                VarSingleton.instance.musicNum;
            DatabaseSingleton.instance.UpdateTable("Chapter", "LastPlayedId", VarSingleton.instance.musicNum
                , "Id", VarSingleton.instance.chapterNum);

            VarSingleton.instance.chapterNum = -1;
            VarSingleton.instance.musicNum = -1;
            VarSingleton.instance.musicNumInChapter = -1;
            VarSingleton.instance.diff = -1;
        }
    }
}
