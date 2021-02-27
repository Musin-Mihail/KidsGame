using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBtn : MonoBehaviour
{
    public enum OPTIONS{SellectScene,MainMenu,ChestScene,ShadowScene,ShapesScene,ShipScene,Level6Chest,Level4Animals,Level5Chest,Level1Shadow,Level2Boat,Level3Shapes}
    public OPTIONS NameScene;
    public void LoadingScene()
    {
        SceneManager.LoadScene(NameScene.ToString());
    }
}
