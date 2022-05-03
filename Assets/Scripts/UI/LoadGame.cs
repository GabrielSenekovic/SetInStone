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
        Game.Instance.background.gameObject.SetActive(false);
        StartCoroutine(LoadLevelAsync());
    }
    IEnumerator LoadLevelAsync()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(2);

        while(!load.isDone)
        {
            loadingBar.value = load.progress;
            yield return null;
        }
    }
}
