using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class ScorePlane : MonoBehaviour
{
    public GameObject SinglePrefab;

    private AudioSource aud;
    private GameObject cam;
    public string eventId;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        aud = cam.GetComponent<AudioSource>();
        Koreographer.Instance.RegisterForEvents(eventId, SingleSpotProduce);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SingleSpotProduce(KoreographyEvent koreographyEvent)
    {
        if (koreographyEvent!= null)
        {
            GameObject tmp = Instantiate(SinglePrefab);
            tmp.transform.position = new Vector3(0, 0, 0);
            tmp.transform.parent = this.transform;
        }
    }
}
