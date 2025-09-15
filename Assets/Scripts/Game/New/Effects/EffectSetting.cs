using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSetting : MonoBehaviour
{
    public Image grade;
    public ParticleSystem effect;

    private void Start()
    {
        Invoke("DestroyThis", 0.6f);
    }

    public void SetGrade(Sprite sprite)
    {
        grade.sprite = sprite;
    }

    public void SetEffectColor(Color color)
    {
        ParticleSystem.MainModule main = effect.main;
        main.startColor = color;
    }

    public void PlayEffect()
    {
        effect.Play();
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
