using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Background : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite[] chapterBack;
    public bool render;

    private int lastChapter;

    // Start is called before the first frame update
    void Start()
    {
        lastChapter = VarSingleton.instance.chapterNum;
        if (lastChapter == -1)
            AlterBack(defaultSprite);
        else
            AlterBack(chapterBack[lastChapter]);
    }

    // Update is called once per frame
    void Update()
    {
        if (VarSingleton.instance.chapterNum != lastChapter)
        {
            lastChapter = VarSingleton.instance.chapterNum;
            if (lastChapter == -1)
                AlterBack(defaultSprite);
            else
                AlterBack(chapterBack[lastChapter]);
        }
    }

    void AlterBack(Sprite sprite)
    {
        if (render == true)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = new Color(1, 1, 1, 0);
            Tweener tweener = renderer.DOFade(1, 0.4f);
            if (lastChapter != -1)
                transform.position = new Vector3(VarSingleton.instance.chapterGap * lastChapter, 0, 100);
        }
        else
        {
            Image renderer = GetComponent<Image>();
            renderer.sprite = sprite;
            renderer.color = new Color(1, 1, 1, 0);
            Tweener tweener = renderer.DOFade(1, 0.4f);
        }
       
    }
}
