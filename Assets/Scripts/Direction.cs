using UnityEngine;

public class Direction : MonoBehaviour
{
    public static Direction Instance { get; private set; }
    public Vector3 direction;
    public AudioClip sound;
    public GameObject bgBlack;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}