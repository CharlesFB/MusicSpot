using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitting : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
    }

}
