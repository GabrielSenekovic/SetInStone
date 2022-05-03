using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Options : MonoBehaviour
{
    public Scrollbar SFXSlider;
    public Scrollbar MusicSlider;
    public Scrollbar VolumeSlider;
    public Scrollbar MapPanSlider;
    public Scrollbar MapZoomSlider;
    public Scrollbar MouseSensitivitySlider;

    private void Awake()
    {
        SFXSlider.value = AudioManager.SFX_volume;
        MusicSlider.value = AudioManager.music_volume;
        VolumeSlider.value = AudioManager.global_volume;
    }

    public void ChangeSFXVolume()
    {
        AudioManager.ChangeSFXVolume(SFXSlider.value);
    }
    public void ChangeMusicVolume()
    {
        AudioManager.ChangeMusicVolume(MusicSlider.value);
    }
    public void ChangeGlobalVolume()
    {
        AudioManager.ChangeGlobalVolume(VolumeSlider.value);
    }
    public void ChangePanSpeed()
    {
        Game.Instance.mapScript.panSpeed = MapPanSlider.value;
    }
    public void ChangeZoomSpeed()
    {
        Game.Instance.mapScript.zoomSpeed = MapZoomSlider.value;
    }
    public void ChangeMouseSensitivity()
    {
        //Game.GetCurrentPlayer().GetComponent<Input>().mouseSensitivity = MouseSensitivitySlider.value;
    }
}
