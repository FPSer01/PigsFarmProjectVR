using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Класс-утилита для дополнительного контроля загрузки/отгрузки сцен
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { private set; get; }

    public static event Action OnSceneLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Перезапустить текущую активную сцену
    /// </summary>
    public void ReloadCurrentScene()
    {
        StartCoroutine(ReloadCurrentScene_Sequence());
    }

    // Перезапустить активную сцену
    private IEnumerator ReloadCurrentScene_Sequence()
    {
        Time.timeScale = 1f;

        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        DOTween.KillAll();

        yield return LoadScene(currentLevelIndex, LoadSceneMode.Single);

        OnSceneLoaded?.Invoke();
    }

    #region Load Methods

    /// <summary>
    /// Подгрузить сцену с показом прогресса загрузки
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="mode"></param>
    /// <param name="progressBar"></param>
    /// <param name="progressStart"></param>
    /// <param name="progressWeight"></param>
    /// <returns></returns>
    public static IEnumerator LoadScene(int sceneIndex, LoadSceneMode mode, Slider progressBar, float progressStart, float progressWeight)
    {
        var operation = SceneManager.LoadSceneAsync(sceneIndex, mode);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (progressBar != null)
            {
                float progress = progressStart + (operation.progress * progressWeight);
                progressBar.value = progress;
            }

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Подгрузить сцену
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static IEnumerator LoadScene(int sceneIndex, LoadSceneMode mode)
    {
        var operation = SceneManager.LoadSceneAsync(sceneIndex, mode);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Отгрузить сцену
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    public static IEnumerator UnloadScene(int sceneIndex)
    {
        var operation = SceneManager.UnloadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Отгрузить сцену
    /// </summary>
    public static IEnumerator UnloadScene(Scene scene)
    {
        var operation = SceneManager.UnloadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    #endregion
}
