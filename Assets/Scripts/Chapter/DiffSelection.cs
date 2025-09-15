using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffSelection : MonoBehaviour
{
    public Info infoController;
    public Toggle easy;
    public Toggle normal;
    public Toggle hard;
    public Toggle insane;

    public void Selected()
    {
        //加载谱面难度信息
        if (easy.isOn)
            infoController.SetDiffAndScores(0);
        else if (normal.isOn)
            infoController.SetDiffAndScores(1);
        else if (hard.isOn)
            infoController.SetDiffAndScores(2);
        else if (insane.isOn)
            infoController.SetDiffAndScores(3);
        else { //特殊谱面待写
        }
    }
}
