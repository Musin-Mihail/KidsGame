using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level11Mouse : MonoBehaviour
{
    public Sprite sprite;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider2D>().enabled = true;
    }
    void OnMouseDown()
    {
        if(gameObject.name == "FishChest")
        {
            Level11.AllFishChestStatic.Add(gameObject);
            // GetComponent<Animator>().enabled = false;
            GetComponent<Animator>().Play("Fish");
            // GetComponent<SpriteRenderer>().sprite = sprite;
            var stars = Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"));
            stars.transform.position = gameObject.transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SoundClickItem>().Play();
            StartCoroutine(Move(Level11.count));
            Level11.count ++;
            Level11.WaitHint = 1;
            gameObject.name = "guessed";
            
        }
        else
        {
            var stars = Instantiate(Resources.Load<ParticleSystem>("BubblesLevel1"));
            stars.transform.position = gameObject.transform.position;
            Level11.WaitHint = 1;
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
        if(WinBobbles.Victory == 1)
        {
            foreach (var item in Level11.Delete)
            {
                Destroy(item);
                yield return new WaitForSeconds(0.02f);
            }
            foreach (var item in Level11.AllFishChestStatic)
            {
                item.GetComponent<Animator>().Play("Scale");
                yield return new WaitForSeconds(0.02f);
            }
            WinBobbles.Victory --;
        }
        else
        {
            WinBobbles.Victory --;
        }
    }
}