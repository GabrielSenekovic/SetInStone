using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    CanvasGroup optionsMenu;
    [SerializeField] CanvasGroup keybindingMenu;
    [SerializeField] CanvasGroup pauseMenu;
    [SerializeField] InputChange inputChange;
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
            OnAwake();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnAwake()
    {
        mapCanvas = mapScript.GetComponent<CanvasGroup>();
        optionsMenu = options.GetComponent<CanvasGroup>();
        
        inputChange.Awake();
    }
    private void Start()
    {
        options.SFXSlider.value = AudioManager.SFX_volume;
        options.MusicSlider.value = AudioManager.music_volume;
        options.VolumeSlider.value = AudioManager.global_volume;

        PlayerInput input = FindObjectOfType<PlayerInput>();
        inputChange.SetDeviceAndScheme(input.devices.ToArray(), input.currentControlScheme);
        inputChange.SetBindingButtons();
    }
    public static HealthBar ConnectToHealthBar()
    {
        return instance.hUD.playerHealthBar;
    }
    public static Text GetTitleText()
    {
        return instance.titleText;
    }
    public void Options()
    {
        pauseMenu.SetCanvasGroupAs(Extensions.SetValue.OFF);
        keybindingMenu.SetCanvasGroupAs(Extensions.SetValue.OFF);
        optionsMenu.SetCanvasGroupAs(Extensions.SetValue.ON);
    }

    public void OpenKeybinding()
    {
        optionsMenu.SetCanvasGroupAs(Extensions.SetValue.OFF);
        keybindingMenu.SetCanvasGroupAs(Extensions.SetValue.TOGGLE);
    }
    public void Pause()
    {
        if (Game.Instance.keybindConflict) { return; } //This variable should be in Input Change, but I don't have time make Input Change a variable of Game right now
        pauseMenu.SetCanvasGroupAs(Extensions.SetValue.TOGGLE);
        optionsMenu.SetCanvasGroupAs(Extensions.SetValue.OFF);
        keybindingMenu.SetCanvasGroupAs(Extensions.SetValue.OFF);
        pause = pauseMenu.alpha == 1;
        Time.timeScale = pause ? 0 : 1;
    }
    public void Map()
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

    public void UpdateCurrency(Inventory.Currency currency, int amount)
    {
        //Check if current area uses this currency. If so, show
        hUD.UpdateCurrency(currency, amount);
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
