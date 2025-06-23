using UnityEngine;

public class Direction : MonoBehaviour
{
    public Vector3 direction;
    public AudioClip Sound;
    public static AudioClip SoundStatic;
    public static Vector3 directionStatic;
    public GameObject BGBlack;
    public static GameObject BGBlackStatic;

    private void Awake()
    {
        directionStatic = direction;
        SoundStatic = Sound;
        BGBlackStatic = BGBlack;
    }
}