using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBtn : MonoBehaviour
{
    public enum OPTIONS{MainMenu,SellectScene,Level1Shadow,Level2Boat,Level3Shapes,Level4Animals,Level5Chest,Level6Chest,Level7Logics,Level8Puzzle,Level9}
    public OPTIONS NameScene;
    public void LoadingScene()
    {
        SceneManager.LoadScene(NameScene.ToString());
    }
}
