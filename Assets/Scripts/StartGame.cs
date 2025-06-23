using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        Invoke("Game", 5.0f);
    }

    private void Game()
    {
        SceneManager.LoadScene("SelectScene");
    }
}