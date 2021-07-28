using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level12Mouse : MonoBehaviour
{
    void OnMouseDown()
    {
        if(Level12.AllTargetStatic.Count > 0 && gameObject.name == Level12.AllTargetStatic[Level12.count].name)
        {
            Level12.WaitHint = 1;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder = 12;
            Particle();
            StartCoroutine(MoveItem(gameObject, Level12.AllTargetStatic[Level12.count]));
            StartCoroutine(ScaleItem(gameObject, Level12.AllTargetStatic[Level12.count]));
            
            // GetComponent<Animator>().Play("Move");
            // Invoke("Particle", 0.0f);
        }
    }
    void Particle()
    {
        GetComponent<SoundClickItem>().Play();
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
    }
    IEnumerator Particle2(int count)
    {
        // yield return new WaitForSeconds(1.5f);
        Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), transform.position, Quaternion.Euler(-90,0,0));
        Level12.AllTargetStatic[count].GetComponent<SpriteRenderer>().enabled = false;
        if(WinBobbles.Victory == 1)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var item in Level12.AllItemStatic)
            {
                
                StartCoroutine(Win(item));
                yield return new WaitForSeconds(0.02f);
            }
            WinBobbles.Victory--;
        }
        else
        {
            WinBobbles.Victory--;
        }
    }
    IEnumerator MoveItem(GameObject item, GameObject target)
    {
        target.GetComponent<Animator>().enabled = false;
        target.transform.localScale = target.GetComponent<Level12Item>()._scale;
        while(item.transform.position != target.transform.position)
        {
            item.transform.position = Vector2.MoveTowards(item.transform.position, target.transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(Particle2(Level12.count));
        Level12.nextFigure();
    }
    IEnumerator ScaleItem(GameObject item, GameObject target)
    {
        while(item.transform.localScale != target.transform.localScale)
        {
            item.transform.localScale = Vector2.MoveTowards(item.transform.localScale, target.transform.localScale, 0.5f);
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator Win(GameObject item)
    {
        var normalScale = item.transform.localScale;
        var bigScale = item.transform.localScale*1.5f;
        while(item.transform.localScale != bigScale)
        {
            item.transform.localScale = Vector2.MoveTowards(item.transform.localScale, bigScale, 0.5f);
            yield return new WaitForSeconds(0.003f);
        }
        while(item.transform.localScale != normalScale)
        {
            item.transform.localScale = Vector2.MoveTowards(item.transform.localScale, normalScale, 0.5f);
            yield return new WaitForSeconds(0.003f);
        }
    }
}