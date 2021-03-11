using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12Mouse : MonoBehaviour
{
    void OnMouseDown()
    {
        if(Level12.AllTargetStatic.Count > 0 && gameObject.name == Level12.AllTargetStatic[Level12.count].name)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = 12;
            GetComponent<Animator>().enabled = true;
            Invoke("Particle", 0.0f);
            Invoke("Particle2", 1.5f);
            Level12.nextFigure();
        }
    }
    void Particle()
    {
        GetComponent<SoundClickItem>().Play();
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
    }
    void Particle2()
    {
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
        WinBobbles.Victory--;
    }
}