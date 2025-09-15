using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectController : MonoBehaviour
{
    public Sprite[] sprites;

    public GameObject effectPrefab;
    public GameObject panel;

    public void SetEffect(Vector3 vec, int type)
    {
        GameObject eff = Instantiate(effectPrefab);
//        Debug.Log(eff.GetComponent<RectTransform>().localScale);
        EffectSetting setting = eff.GetComponent<EffectSetting>();
        eff.transform.SetParent(panel.transform, false);
        //Debug.Log(eff.GetComponent<RectTransform>().localScale);
        eff.transform.position = vec;
       
        switch (type)
        {
            case 0: //perfect
                setting.SetEffectColor(new Color(255,255,130,1));
                setting.PlayEffect();
                break;
            case 1: //great
                setting.SetEffectColor(new Color(130,210,255,1));
                setting.PlayEffect();
                break;
        }
        setting.SetGrade(sprites[type]);
    }
}
