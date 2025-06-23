using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBtn : MonoBehaviour
{
    public enum OPTIONS
    {
        MainMenu,
        SelectScene,
        Level1Shadow,
        Level2Boat,
        Level3Shapes,
        Level4Animals,
        Level5Chest,
        Level6Chest,
        Level7Logics,
        Level8Puzzle,
        Level9,
        Level10,
        Level11,
        Level12
    }

    public OPTIONS NameScene;

    public void LoadingScene()
    {
        SceneManager.LoadScene(NameScene.ToString());
    }
}