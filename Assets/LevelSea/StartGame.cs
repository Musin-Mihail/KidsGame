using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        Invoke("Game", 5.0f);
    }
    void Game()
    {
        SceneManager.LoadScene("SellectScene");
    }
}