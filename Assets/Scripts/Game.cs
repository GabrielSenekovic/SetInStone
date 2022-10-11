using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public GameObject player;
    public Volume volume;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public InputChange inputChange;

    public bool keybindConflict = false; //If theres a keybinding conflict

    public Camera cameraFollowsMainCamera;

    public GameObject audioManagerPrefab;
    public static GameObject audioManager;
    static bool gameStarted = false;
    public Map mapScript;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if(!gameStarted)
        {
            gameStarted = true;
            audioManager = Instantiate(audioManagerPrefab, transform.position, Quaternion.identity);
            instance.volume = instance.GetComponentInChildren<Volume>();
        }
    }

    public static void AttachPlayer(GameObject playerIn)
    {
        instance.player = playerIn;
        instance.cinemachineVirtualCamera.m_Follow = playerIn.transform;
        instance.cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = playerIn.transform.parent.GetComponent<Movement>().room;
    }
    public static GameObject GetCurrentPlayer()
    {
        return instance.player;
    }
    public static void GameOver()
    {
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

    public static Volume GetVolume()
    {
        return instance.volume;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
