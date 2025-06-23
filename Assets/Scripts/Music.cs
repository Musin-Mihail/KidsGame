using UnityEngine;

public class Music : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}