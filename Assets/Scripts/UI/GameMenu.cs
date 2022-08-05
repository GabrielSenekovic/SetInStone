using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    static GameMenu instance;
    public static GameMenu Instance
    {
        get
        {
            return instance;
        }
    }
    [SerializeField] Options options;
    [SerializeField] CanvasGroup keybindingMenu;
    [System.NonSerialized] public CanvasGroup mapCanvas;
    public Map mapScript;
    bool pause = false;
    public HUD hUD;
    public Text titleText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            mapCanvas = mapScript.GetComponent<CanvasGroup>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        options.SFXSlider.value = AudioManager.SFX_volume;
        options.MusicSlider.value = AudioManager.music_volume;
        options.VolumeSlider.value = AudioManager.global_volume;
    }
    public static HealthBar ConnectToHealthBar()
    {
        return Instance.hUD.playerHealthBar;
    }
    public static Text GetTitleText()
    {
        return instance.titleText;
    }
    public void Options()
    {
        gameObject.SetActive(options.gameObject.activeSelf);
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
    public void Pause()
    {
        if (Game.Instance.keybindConflict) { return; } //This variable should be in Input Change, but I don't have time make Input Change a variable of Game right now
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        gameObject.SetActive(pause);
        options.gameObject.SetActive(false);
        keybindingMenu.alpha = 0;
        keybindingMenu.interactable = false;
        keybindingMenu.blocksRaycasts = false;
    }
    public void Menu()
    {
        if (mapCanvas.alpha == 0)
        {
            mapScript.Open();
        }
        else
        {
            mapScript.Close();
        }
        Game.Instance.player.GetComponent<Input>().SetControllable(mapCanvas.alpha == 0);
        hUD.GetComponent<CanvasGroup>().alpha = mapCanvas.alpha;
    }
    public void StartMenu()
    {
        DontDestroyOnLoad(Game.audioManager);
        DontDestroyOnLoad(this);
        Game.Instance.cinemachineVirtualCamera.m_Follow = null;
        Game.Instance.cinemachineVirtualCamera.gameObject.SetActive(false);
        titleText.text = "";
        Time.timeScale = 1;
        SceneManager.LoadScene("Start Menu", LoadSceneMode.Single);
        transform.position = Vector2.zero;
        Game.Instance.cameraFollowsMainCamera.transform.position = new Vector3(-11.58f, -6.46f, -10);
        ColorAdjustments color;
        Game.Instance.volume.sharedProfile.TryGet<ColorAdjustments>(out color);
        color.colorFilter.value = Color.white;
    }
    public static IEnumerator WaitForLoad(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        instance.StartMenu();
    }
    public void Zoom(Vector2 value)
    {
        if (mapCanvas.alpha == 1)
        {
            mapScript.Zoom(value);
        }
    }
    public void Quit()
    {
        Game.Instance.Quit();
    }
}
