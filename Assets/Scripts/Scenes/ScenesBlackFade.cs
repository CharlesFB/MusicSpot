using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScenesBlackFade : MonoBehaviour
{
    public SpriteRenderer blackScene;
    void Start()
    {
        blackScene.sortingOrder = 10;
        blackScene.DOFade(0, 0.6f);
    }

    public void blackFadeIn()
    {
        blackScene.DOFade(1, 0.8f);
    }
}
