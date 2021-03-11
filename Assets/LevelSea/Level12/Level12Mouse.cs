using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12Mouse : MonoBehaviour
{
    // private void Start() 
    // {
    //     gameObject.name = "Item";
    // }
    void OnMouseDown()
    {
        if(Level12.AllTargetStatic.Count > 0 && gameObject.name == Level12.AllTargetStatic[Level12.count].name)
        {
            // gameObject.name = "ItemOld";
            Level12.WaitHint = 1;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = 12;
            // GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("Move");
            Invoke("Particle", 0.0f);
            StartCoroutine(Particle2(Level12.count));
            
            Level12.nextFigure();
        }
    }
    void Particle()
    {
        GetComponent<SoundClickItem>().Play();
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
    }
    IEnumerator Particle2(int count)
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
        Level12.AllTargetStatic[count].GetComponent<SpriteRenderer>().enabled = false;
        if(WinBobbles.Victory == 1)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var item in Level12.AllItemStatic)
            {
                item.GetComponent<Animator>().Play("Scale");
                yield return new WaitForSeconds(0.02f);
            }
            WinBobbles.Victory--;
        }
        else
        {
            WinBobbles.Victory--;
        }
    }
}