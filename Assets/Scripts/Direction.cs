using UnityEngine;

public class Direction : MonoBehaviour
{
    public static Direction instance { get; private set; }
    public GameObject bgBlack;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}