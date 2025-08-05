using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music instance { get; private set; }

    public void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}