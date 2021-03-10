using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12Mouse : MonoBehaviour
{
    void OnMouseDown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder = 12;
        GetComponent<Animator>().enabled = true;
        Invoke("Particle", 1.5f);
    }
    void Particle()
    {
        GetComponent<SoundClickItem>().Play();
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,-40,0));
        WinBobbles.Victory--;
    }
}