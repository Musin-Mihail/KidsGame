using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private readonly WaitForSeconds _loadDelay = new(5.0f);

    private void Start()
    {
        StartCoroutine(LoadSelectSceneAfterDelay());
    }

    /// <summary>
    /// Загружает сцену выбора уровня после задержки.
    /// </summary>
    private IEnumerator LoadSelectSceneAfterDelay()
    {
        yield return _loadDelay;
        SceneManager.LoadScene("SelectScene");
    }
}