using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private LoadingUI loadingUI;
    private BaseScene currentScene;
    public BaseScene CurrentScene
    {
        get
        {
            if (currentScene == null)
                currentScene = GameObject.FindObjectOfType<BaseScene>();

            return currentScene;
        }
    }
    
    private void Awake()
    {
        LoadingUI loadingUI = Resources.Load<LoadingUI>("UI/LoadingUI");
        this.loadingUI = Instantiate(loadingUI);
        this.loadingUI.transform.SetParent(transform);
    }
    public void LoadScene(string sceneName, int loadingImage)
    {
        StartCoroutine(LoadingRoutine(sceneName, loadingImage));
    }
    IEnumerator LoadingRoutine(string sceneName, int loadingImage)
    {
        loadingUI.SetProgress(0f);
        loadingUI.FadeOut();
        loadingUI.SetImage(loadingImage);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (!oper.isDone)
        {
            loadingUI.SetProgress(Mathf.Lerp(0.0f, 0.5f, oper.progress));
            yield return null;
        }
        if (CurrentScene != null)
        {
            CurrentScene.LoadAsync();
            while (CurrentScene.progress < 1f)
            {
                loadingUI.SetProgress(Mathf.Lerp(0.5f, 1f, CurrentScene.progress));
                yield return null;
            }
        }
        loadingUI.SetProgress(1f);
        loadingUI.FadeIn();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
    }
}
