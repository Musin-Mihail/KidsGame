using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4MouseClick : MonoBehaviour
{
    public Vector3 Position;

    int layerMask = 1 << 10;

    void OnMouseDown()
    {
        Position = GetComponent<MoveItem>().StartPosition;
        MouseMove.MoveFigures = gameObject;
        Level4Global.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.tag == gameObject.tag)
            {
                Transform[] allChildren = hitColliders.GetComponentsInChildren<Transform>();
                foreach (var item in allChildren)
                {
                    if(item.name == gameObject.name)
                    {
                        GetComponent<BoxCollider2D>().enabled = false;
                        MouseMove.MoveFigures = null;
                        StartCoroutine(Move(item));
                        break;
                    }
                }
            }
            else
            {
                transform.position = Position;
                MouseMove.MoveFigures = null;
            }
        }
        else
        {
            transform.position = Position;
            MouseMove.MoveFigures = null;
        }
    }
    IEnumerator Move(Transform item)
    {
        item.GetComponent<SoundClickItem>().Play();
        Level4Global.AllCollected.Add(item.gameObject);
        while(transform.position != item.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, item.transform.position, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        if(item.tag == "Water")
        {
            var newVector3 = item.transform.position;
            newVector3.z += 0.5f;
            Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
        }
        if(item.name == "Dog")
        {
             Transform[] allChildren = item.GetComponentsInChildren<Transform>();
             foreach (var item2 in allChildren)
             {
                 item2.GetComponent<SpriteRenderer>().enabled = true;
             }
        }
        item.GetComponent<SpriteRenderer>().enabled = true;
        // yield return new WaitForSeconds(1);
        Destroy(gameObject);
        WinBobbles.Victory --; 
    }
}