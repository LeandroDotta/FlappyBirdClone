using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public enum FadeStyle
    {
        FadeOutAndIn,
        FadeInAndOut,
        OnlyFadeIn,
        OnlyFadeOut
    }

    public float duration;
    public Texture texture;
    public FadeStyle style;

    private bool fadeIn;
    private bool fadeOut;
    private bool isFading;

    private float counter;
    private float alphaValue;
    private Color fadeColor;

    // Eventos
    public delegate void FadeInAction();
    public event FadeInAction OnFadeIn;
    public delegate void FadeOutAction();
    public event FadeOutAction OnFadeOut;

    void Start()
    {
        fadeColor = GUI.color;
    }

    void Update()
    {
        if (isFading)
        {
            counter += Time.deltaTime;
            alphaValue = counter / duration;
        }
    }

    void OnGUI()
    {
        if (isFading)
        {
            if (fadeIn)
            {
                FadeIn();
            }
            else if (fadeOut)
            {
                FadeOut();
            }

            if (alphaValue >= 1)
            {
                counter = 0;
                alphaValue = 0;

                if (fadeIn)
                {
                    fadeIn = false;

                    if (OnFadeIn != null)
                        OnFadeIn.Invoke();

                    if (style == FadeStyle.FadeInAndOut)
                        fadeOut = true;
                }
                else if (fadeOut)
                {
                    fadeOut = false;

                    if (OnFadeOut != null)
                        OnFadeOut.Invoke();

                    if (style == FadeStyle.FadeOutAndIn)
                        fadeIn = true;
                }
                
                if (!fadeIn && !fadeOut)
                {
                    isFading = false;
                }
            }

            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
    }

    public void Fade()
    {
        switch (style)
        {
            case FadeStyle.FadeInAndOut:
            case FadeStyle.OnlyFadeIn:
                fadeIn = true;
                break;

            case FadeStyle.FadeOutAndIn:
            case FadeStyle.OnlyFadeOut:
                fadeOut = true;
                break;
        }

        isFading = true;
    }

    void FadeOut()
    {
        fadeColor = GUI.color;
        fadeColor.a = alphaValue;
    }

    void FadeIn()
    {
        fadeColor = GUI.color;
        fadeColor.a = 1 - alphaValue;
    }
}