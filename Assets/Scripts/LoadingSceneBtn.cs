using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBtn : MonoBehaviour
{
    public string GetNameScene;

    public void LoadingScene()
    {
        SceneManager.LoadScene(GetNameScene);
    }
}
