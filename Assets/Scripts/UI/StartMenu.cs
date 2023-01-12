using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Options options;
    [SerializeField] CanvasGroup keybindingMenu;
    [SerializeField] InputChange inputChange;

    private void Awake()
    {
        inputChange.Awake();
        PlayerInput input = FindObjectOfType<PlayerInput>();
        inputChange.SetDeviceAndScheme(input.devices.ToArray(), input.currentControlScheme);
    }
    private void Start()
    {
        options.SFXSlider.value = AudioManager.SFX_volume;
        options.MusicSlider.value = AudioManager.music_volume;
        options.VolumeSlider.value = AudioManager.global_volume;

        inputChange.SetBindingButtons();
    }
    public void Options()
    {
        options.gameObject.SetActive(options.gameObject.activeSelf ? false : true);
    }
    public void OpenKeybinding()
    {
        if (options)
        {
            options.gameObject.SetActive(keybindingMenu.alpha == 1);
        }
        keybindingMenu.alpha = keybindingMenu.alpha == 1 ? 0 : 1;
        keybindingMenu.interactable = !options.gameObject.activeSelf;
        keybindingMenu.blocksRaycasts = keybindingMenu.interactable;
    }
    public void LoadLevel()
    {
        Time.timeScale = 1;
        DontDestroyOnLoad(Game.audioManager);
        DontDestroyOnLoad(Game.Instance);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        AudioManager.PlayMusic("ForestDungeonTheme", true);
    }
    public void Quit()
    {
        Game.Instance.Quit();
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
