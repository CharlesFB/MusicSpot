using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public bool isOkForTouch;

    /*
     * type = 0: single spot
     * type = 1: touch spot
     * type = 2: press spot 1/4倍上下屏距
     * type = 3: press spot 1/2倍上下屏距
     * type = 4: press spot 1倍上下屏距
     */
    public int type;
    public float animSpeed = 1;

    protected void endAnim()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
