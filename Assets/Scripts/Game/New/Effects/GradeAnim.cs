using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GradeAnim : MonoBehaviour
{

    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOLocalMove(new Vector3(rt.anchoredPosition.x,rt.anchoredPosition.y+20),0.5f);
        GetComponent<Image>().DOFade(1, 0.1f);
        Invoke("FadeOut",0.4f);
    }

    void FadeOut()
    {
        GetComponent<Image>().DOFade(0, 0.1f);
    }
}
