using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBtn : MonoBehaviour
{
    public enum Options
    {
        MainMenu,
        SelectScene,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10,
        Level11,
        Level12
    }

    public Options NameScene;

    public void LoadingScene()
    {
        SceneManager.LoadScene(NameScene.ToString());
    }
}