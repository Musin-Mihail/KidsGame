using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11Mouse : MonoBehaviour
{
    public Sprite sprite;
    // Vector3 Position;
    // int layerMask = 1 << 9;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    void OnMouseDown()
    {
        if(gameObject.name == "FishChest")
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = sprite;
            var stars = Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"));
            stars.transform.position = gameObject.transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SoundClickItem>().Play();
            StartCoroutine(Move(Level11.count));
            Level11.count ++;
        }
        else
        {
            var stars = Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"));
            stars.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }
    IEnumerator Move(int count)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 3;
        var target = Level11.AllTargetStatic[count].transform.position;
        while(transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
            yield return new WaitForSeconds(0.005f);
        }
        Level11.AllTargetStatic[count].GetComponent<SpriteRenderer>().sprite = null;
        WinBobbles.Victory --;
        if(WinBobbles.Victory == 0)
        {
            foreach (var item in Level11.Delete)
            {
                Destroy(item);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}