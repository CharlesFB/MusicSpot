using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLineFitting : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * 1.4f);
    }
}
