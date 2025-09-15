using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private int length;
    public AudioSource aud;

    // Start is called before the first frame update
    void Start()
    {
        length = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (aud.isPlaying)
        {
            RectTransform tr = this.GetComponent<RectTransform>();
            tr.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, length * aud.time / aud.clip.length);
        }
    }
}
