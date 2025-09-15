using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeFitting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
