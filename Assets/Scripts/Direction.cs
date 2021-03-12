using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public Vector3 direction;
    public AudioClip Sound;
    public static AudioClip SoundStatic;
    public static Vector3 directionStatic;
    public GameObject BGBlack;
    public static GameObject BGBlackStatic;
    public GameObject CanvasBubbles;
    public static GameObject CanvasBubblesStatic;
    
    void Awake()
    {
        directionStatic = direction;
        SoundStatic = Sound;
        BGBlackStatic = BGBlack;
        CanvasBubblesStatic = CanvasBubbles;
    }
}