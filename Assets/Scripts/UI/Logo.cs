using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ToChapter", 2);
    }

    private void ToChapter()
    {
        SceneManager.LoadScene("ChapterPage");
    }
}
