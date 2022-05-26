using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    static Game instance;
    public static Game Instance
    {
        get
        {
            return instance;
        }
    }

    GameObject player;
    public Volume volume;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public InputChange inputChange;

    public GameObject pauseMenu;
    public GameObject startMenu;
    public CanvasGroup keybindingMenu;

    bool pause = false;
    public bool keybindConflict = false; //If theres a keybinding conflict
    [System.NonSerialized] public CanvasGroup mapCanvas;
    public Map mapScript;

    public Text titleText;

    public Options options;

    public Camera cameraFollowsMainCamera;

    public GameObject audioManagerPrefab;
    static GameObject audioManager;
    static bool gameStarted = false;

    public Image background;

    public bool startMenuOn;

    public HUD hUD;
    public Image voidImage; //The image behind the pixel filter

    private void Awake()
    {
        if(instance == null)
        {
            Debug.Log("Awake called on Game script");
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if(!gameStarted)
        {
            Debug.Log("loaded game scene");
            gameStarted = true;
            audioManager = Instantiate(audioManagerPrefab, transform.position, Quaternion.identity);

            Debug.Log("Initialized Game Script");

            Debug.Log("Getting Volume");
            instance.volume = instance.GetComponentInChildren<Volume>();

            mapCanvas = mapScript.GetComponent<CanvasGroup>();

            instance.options.SFXSlider.value = AudioManager.SFX_volume;
            instance.options.MusicSlider.value = AudioManager.music_volume;
            instance.options.VolumeSlider.value = AudioManager.global_volume;
        }
        
    }

    public static void AttachPlayer(GameObject playerIn)
    {
        instance.player = playerIn;
        instance.cinemachineVirtualCamera.m_Follow = playerIn.transform;
        instance.inputChange.Initialize(playerIn.GetComponent<PlayerInput>().currentControlScheme);
    }
    public static GameObject GetCurrentPlayer()
    {
        return instance.player;
    }
    public static void GameOver()
    {
        Debug.Log("Game Over");
        //Trigger game over screen
    }

    public static Collider2D GetCurrentCameraCollider()
    {
        return instance.cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
    }
    public static void SetCameraCollider(Collider2D collider)
    {
        instance.cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider;
    }
    public void Pause()
    {
        if (keybindConflict) { return; } //This variable should be in Input Change, but I don't have time make Input Change a variable of Game right now
        pause = !pause;
        Time.timeScale = pause ? 0: 1;
        pauseMenu.SetActive(pause);
        options.gameObject.SetActive(false);
        keybindingMenu.alpha = 0;
        keybindingMenu.interactable = false;
        keybindingMenu.blocksRaycasts = false;
    }
    public void Menu()
    {
        if(mapCanvas.alpha == 0)
        {
            mapScript.Open();
        }
        else
        {
            mapScript.Close();
        }
        player.GetComponent<Input>().SetControllable(mapCanvas.alpha == 0);
        Game.SetHUDVisibility((int)mapCanvas.alpha == 0? 1: 0);
    }

    public void Options()
    {
        if(pauseMenu && !startMenuOn)
        {
            pauseMenu.SetActive(options.gameObject.activeSelf);
        }
        else if(startMenuOn && startMenu)
        {
            startMenu.SetActive(options.gameObject.activeSelf);
        }
        options.gameObject.SetActive(options.gameObject.activeSelf ? false : true);
    }
    public void Options_StartMenu()
    {
        if(startMenu)
        {
            startMenu.SetActive(options.gameObject.activeSelf);
        }
        options.gameObject.SetActive(options.gameObject.activeSelf ? false : true);
    }
    public void OpenKeybinding()
    {
        if(options)
        {
            options.gameObject.SetActive(keybindingMenu.alpha == 1);
        }
        keybindingMenu.alpha = keybindingMenu.alpha == 1 ? 0 : 1;
        keybindingMenu.interactable = !options.gameObject.activeSelf;
        keybindingMenu.blocksRaycasts = keybindingMenu.interactable;
    }

    public void Zoom(Vector2 value)
    {
        if(mapCanvas.alpha == 1)
        {
            mapScript.Zoom(value);
        }
    }
    public static void CreateMapTexture(List<Map.RoomData> tiles)
    {
        instance.mapScript.CreateMapTexture(tiles);
    }
    public static Vector2 GetPlayerPosition()
    {
        if(instance != null && instance.player != null)
        {
            return instance.player.transform.position;
        }
        return Vector2.zero;
    }

    public void StartMenu()
    {
        background.gameObject.SetActive(true);
        DontDestroyOnLoad(audioManager);
        DontDestroyOnLoad(this);
        Instance.startMenuOn = true;
        hUD.TurnOff();
        titleText.text = "";
        SceneManager.LoadScene("Start Menu", LoadSceneMode.Single);
        SetHUDVisibility(0);
        startMenu.SetActive(true);
    }

    public void LoadLevel()
    {
        Time.timeScale = 1;
        Object.DontDestroyOnLoad(audioManager);
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        Instance.startMenuOn = false;
        SetHUDVisibility(1);
        AudioManager.PlayMusic("ForestDungeonTheme", true);
    }

    public static Volume GetVolume()
    {
        return instance.volume;
    }
    public static Text GetTitleText()
    {
        return instance.titleText;
    }
    public static HealthBar ConnectToHealthBar()
    {
        return instance.hUD.playerHealthBar;
    }

    public static void SetHUDVisibility(int value)
    {
        instance.hUD.canvas.alpha = value;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static IEnumerator WaitForLoad(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        instance.StartMenu();
    }
}
