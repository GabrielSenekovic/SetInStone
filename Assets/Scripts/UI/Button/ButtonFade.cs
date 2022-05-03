using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ButtonFade : MonoBehaviour
{
    float fadeSpeed;
    int fadeTimer = 0;
    int VFXTimer = 0;
    [SerializeField] int VFXTimer_Max;
    [SerializeField] int fadeWaitingTime; //Amount of time to wait before the button starts fading
    [SerializeField] VisualEffect fadeEffect;
    [SerializeField] float VFXmoveModifier;

    [SerializeField] float positionAtRatioOne; //The value that would be used if the ratio was 1. 
    //It's used to calculate the position in different resolutions

    float rightEndPosition;
    Material material;

    // public Vector2 currentRes;

    private void Start()
    {
        material = GetComponent<Image>().material;
        material.SetFloat("Fill", 10);
    }

    private void FixedUpdate() 
    {
        //9.5f was originally used for position at ratio one
        //It then decreases by 0.5f for every 0.1f of the ratio after that
        float ratio = (float)Screen.width / (float)Screen.height;
        float change = (ratio - 1.0f) * 5.0f;
        fadeSpeed = 0.01f + (0.01f - (ratio - 1.0f) / 78.0f);
       // VFXmoveSpeed = 0.01f + (0.01f - (ratio - 1.0f) / VFXmoveModifier);
        rightEndPosition = positionAtRatioOne - change;

       // currentRes = new Vector2(Screen.width, Screen.height);
            //new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        if(fadeTimer == 0 || fadeEffect.GetFloat("Fade Position") == -rightEndPosition){return;} 
        //*^^^Don't run the code unless the button has been pressed
        if(fadeTimer < fadeWaitingTime){fadeTimer++;}
        else
        {
            Fade();
        }
    }
    void Fade()
    {
        float valueToDecrease = fadeSpeed * 2 * GetComponent<RectTransform>().localScale.x;
        material.SetFloat("Fill", material.GetFloat("Fill") - valueToDecrease);
        if(VFXTimer < VFXTimer_Max) { VFXTimer++; }
        else
        {
            MoveVFX();
        }
    }
    void MoveVFX()
    {
        if (fadeEffect.GetInt("Rate") == 0) { fadeEffect.SetInt("Rate", 2000); } //Setting spawn rate of particles

        float valueToDecrease = fadeSpeed *
        (2/*because 0 is the middle of the button, so it needs to decrease twice as fast*
        /*added because the edges are 0.7f further out*/) *
        GetComponent<RectTransform>().localScale.x;

        if (fadeEffect.GetFloat("Fade Position") - valueToDecrease < -rightEndPosition) { fadeEffect.SetFloat("Fade Position", -rightEndPosition); fadeEffect.SetInt("Rate", 0); }
        else { fadeEffect.SetFloat("Fade Position", fadeEffect.GetFloat("Fade Position") - valueToDecrease); }
    }
    public void FadeButton()
    {
        material.SetFloat("Fill", 10);
        material.SetFloat("Fill", 10);
        fadeEffect.SetFloat("Fade Position", rightEndPosition);
        fadeTimer++;
    }
}
