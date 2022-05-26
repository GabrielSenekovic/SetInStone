using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    [SerializeField]Slider loadingBar;

    private void Start() 
    {
        StartCoroutine(LoadLevelAsync());
    }
    IEnumerator LoadLevelAsync()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(2);
        Game.Instance.cinemachineVirtualCamera.gameObject.SetActive(true);

        while(!load.isDone)
        {
            loadingBar.value = load.progress;
            yield return null;
        }
    }
}
