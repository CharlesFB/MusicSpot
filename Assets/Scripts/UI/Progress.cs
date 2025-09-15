using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{
    private int length;
    private GameObject cam;
    private AudioSource aud;

    // Start is called before the first frame update
    void Start()
    {
        length = Screen.width;
        cam = GameObject.Find("Main Camera");
        aud = cam.GetComponent<AudioSource>();
        
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
