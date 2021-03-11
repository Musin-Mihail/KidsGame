using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6MouseClick : MonoBehaviour
{
    public Vector3 Position;
    int layerMask = 1 << 6;
    void OnMouseDown()
    {
        Position = GetComponent<MoveItem>().StartPosition;
        Level6Global.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.tag == gameObject.tag)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                var place = hitColliders.GetComponent<Level6Chest>().BusyPlaces;
                var GO = new GameObject();
                GO.transform.parent = hitColliders.transform;
                GO.transform.localPosition = hitColliders.GetComponent<Level6Chest>().CollectedThings[place];
                GO.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                GO.AddComponent<SpriteRenderer>();
                GO.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                GO.AddComponent<WinUp>();
                Level6Global.AllCollectedStars.Add(GO);
                var newVector3 = GO.transform.position;
                newVector3.z = 2.5f;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                hitColliders.GetComponent<Level6Chest>().BusyPlaces ++;
                Destroy(gameObject);
                WinBobbles.Victory --; 
            }
            else
            {
                gameObject.GetComponent<MoveItem>().State = 1;
                transform.position = Position;
            }
        }
        else
        {
            gameObject.GetComponent<MoveItem>().State = 1;
            transform.position = Position;
        }
    }
    void OnMouseDrag()
    {
        if(Input.GetMouseButton(0))
        {
            var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _newVector2.z = 0;
            transform.position = _newVector2;
        }
        // if(Input.touchCount > 0)
        // {
        //     var _newVector2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        //     _newVector2.z = 0;
        //     transform.position = _newVector2;
        // }
    }
}