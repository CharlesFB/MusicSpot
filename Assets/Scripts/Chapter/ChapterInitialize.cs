using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChapterInitialize : MonoBehaviour
{
    public GameObject chapterPrefab;
    public Sprite[] metas;

    public CameraAutoMove camera;
    public GameObject back;
    public Info infoController;
    //public Transform visibleMusic;

    // Start is called before the first frame update
    void Start()
    {
//        Debug.Log(VarSingleton.instance.chapterCount);
        //动态加载所有的章节
        for (int i = 0; i < VarSingleton.instance.chapterCount; i++)
        {
            GameObject tmp = Instantiate(chapterPrefab);
            ChapterLoad load = tmp.GetComponent<ChapterLoad>();
            load.number = i;
            load.ChangeName(VarSingleton.instance.chapters[i].chapterName);
            load.LoadImg(metas[VarSingleton.instance.chapters[i].lastPlayedId]);
            tmp.transform.position = new Vector3(i* VarSingleton.instance.chapterGap, 0, 15);

            load.metas = new Sprite[VarSingleton.instance.chapters[i].musicNum];
            load.musics = new VarSingleton.Music[VarSingleton.instance.chapters[i].musicNum];
            int j = 0;
            while (VarSingleton.instance.musics[j] != null)
            {
                if (VarSingleton.instance.musics[j].chapterId == i)
                {
                    //Debug.Log(VarSingleton.instance.musics[j].clipId);
                    load.metas[VarSingleton.instance.musics[j].clipId] = metas[j];
                    load.musics[VarSingleton.instance.musics[j].clipId] = VarSingleton.instance.musics[j];
                }
                j++;
            }
        }

        if (VarSingleton.instance.chapterNum != -1)
        {
            camera.CameraMove(VarSingleton.instance.chapterNum);
            infoController.SetName();
            infoController.SetDiffAndScores(VarSingleton.instance.diff);
            infoController.MoveIn();
            back.GetComponent<Image>().DOFade(1, 0.4f);
            back.GetComponent<Button>().enabled = true;

            //遍历所有的visiblemusic组件，寻找对应当前章节编号的那一个，scale打开(1,1,1)
        }
    }

}
