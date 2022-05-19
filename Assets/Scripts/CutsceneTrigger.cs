using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CutsceneTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    ColorAdjustments color;

    float fadeSpeed = 0.005f;
    float yellowShiftSpeed = 1.7f;

    int cutsceneState = 0; //Different for different types of cutscnees what the different numbers do
    //0 == nothing
    //1 == fade
    //2 == write

    int writeTimer = 0;
    [SerializeField]int writeTimerMax;

    [SerializeField]string text;
    int textIndex = 0;

    private void Start() 
    {
        VolumeProfile profile = Game.GetVolume().sharedProfile;
        profile.TryGet<ColorAdjustments>(out color);
        color.colorFilter.value = Color.white;
    }

    private void FixedUpdate() 
    {
        if(cutsceneState == 1)
        {
            color.colorFilter.value = new Color(color.colorFilter.value.r-fadeSpeed/yellowShiftSpeed, 
                                                color.colorFilter.value.g-fadeSpeed/yellowShiftSpeed, 
                                                color.colorFilter.value.b-fadeSpeed, color.colorFilter.value.a);
            if(color.colorFilter.value.r <= 0)
            {
                cutsceneState = 2;
            }
        }
        else if(cutsceneState == 2)
        {
            writeTimer++;
            if(writeTimer >= writeTimerMax && Game.GetTitleText().text != text)
            {
                writeTimer = 0;
                Game.GetTitleText().text += text[textIndex];
                textIndex++;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player") && cutsceneState == 0)
        {
            cutsceneState = 1;
            Debug.Log("Collided");
            Input input = other.GetComponent<Input>();
            if(input == null)
            {
                input = other.transform.parent.GetComponent<Input>();
            }
            input.SetControllable(false);
            AudioManager.PlayMusic("Dungeon Clear Last", false);
            //StartCoroutine(Game.WaitForLoad(AudioManager.GetMusic("Dungeon Clear Last").theme.length));
            Game.SetHUDVisibility(0);
        }
    }
}
