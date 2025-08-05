using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSelectSceneAfterDelay(5.0f));
    }

    /// <summary>
    /// Загружает сцену выбора уровня после указанной задержки.
    /// </summary>
    /// <param name="delay">Время ожидания в секундах.</param>
    private IEnumerator LoadSelectSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SelectScene");
    }
}