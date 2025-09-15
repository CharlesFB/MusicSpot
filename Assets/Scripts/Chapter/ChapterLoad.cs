using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChapterLoad : MonoBehaviour
{
    public int number;//这个是第几章

    public GameObject visibleMusic;

    public Sprite[] metas; //本章音乐meta
    public VarSingleton.Music[] musics; //本章全部音乐 

    void Start()
    {
        visibleMusic.GetComponent<VisibleMusic>().chapterNumber = number;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (VarSingleton.instance.chapterNum == -1)//之前未选中任何章节
            {
                //切换章节
                VarSingleton.instance.chapterNum = number;
                //切换曲目
                int num = VarSingleton.instance.musicNum = VarSingleton.instance.chapters[number].lastPlayedId;
                VarSingleton.instance.musicNumInChapter = VarSingleton.instance.musics[num].clipId;

                //加载列表(滑动)

                //返回按键出现
                GameObject back = GameObject.Find("Back");
                back.GetComponent<Image>().DOFade(1, 0.4f);
                back.GetComponent<Button>().enabled = true;

                Info infoController = GameObject.Find("Canvas").GetComponent<Info>();
                //加载乐曲信息
                infoController.SetName();
                infoController.SetDiffAndScores(VarSingleton.instance.musics[num].lastPlayedDiff);
                //显示UI两边
                infoController.MoveIn();

                //加载可视化音乐
                visibleMusic.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (VarSingleton.instance.chapterNum == number)
            {
                int total = VarSingleton.instance.chapters[number].musicNum;
                int num = VarSingleton.instance.musicNumInChapter;
                num = (num + 1) % total;
                VarSingleton.instance.musicNumInChapter = num; //设置当前章节内的曲目序号
                SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer s in srs) //更改图片
                {
                    if (s.name == "Image")
                    {
                        s.sprite = metas[num];
                        break;
                    }
                }

                for (int i = 0; i < VarSingleton.instance.musics.Length; i++)
                {
                    if (VarSingleton.instance.musics[i] == null)
                        break;
                    if (VarSingleton.instance.musics[i].clipId == num && VarSingleton.instance.musics[i].chapterId == number)
                    {
                        num = i;
                        break;
                    }
                }

                VarSingleton.instance.musicNum = num; //设置当前播放的曲目ID
                VarSingleton.instance.chapters[number].lastPlayedId = num; //修改上次游玩的id
                DatabaseSingleton.instance.UpdateTable("Chapter","lastPlayedId",num,"Id",number);//修改数据库

                Info infoController = GameObject.Find("Canvas").GetComponent<Info>();
                //加载乐曲信息
                infoController.SetName();
                infoController.SetDiffAndScores(VarSingleton.instance.musics[num].lastPlayedDiff);
            }
        }
        
    }

    public void ChangeName(string name) //更改章节名称
    {
        this.name = name;
    }

    public void LoadImg(Sprite sprite)  //加载章节meta
    {
        foreach(SpriteRenderer img in GetComponentsInChildren<SpriteRenderer>())
        {
            if (img.name == "Image")
            {
                img.sprite = sprite;
            }
        }
    }

    public void SetPosition(Vector2 pos) //设置位置
    {
        transform.position = pos;
    }


}
