using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6MouseClick : MonoBehaviour
{
    public Vector3 Position;
    int layerMask = 1 << 6;
    void OnMouseDown()
    {
        Position = transform.position;
        Level6MouseMove.MoveFigures = gameObject;
        Level6Global.WaitHint = 1;
        gameObject.GetComponent<Level6StarMove>().State = 0;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.tag == gameObject.tag)
            {
                hitColliders.GetComponent<Level6Chest>().PlayBell();
                var place = hitColliders.GetComponent<Level6Chest>().BusyPlaces;
                var GO = new GameObject();
                GO.transform.parent = hitColliders.transform;
                GO.transform.localPosition = hitColliders.GetComponent<Level6Chest>().CollectedThings[place];
                GO.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                GO.AddComponent<SpriteRenderer>();
                GO.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                GO.AddComponent<Level6WinStar>();
                Level6Global.AllCollectedStars.Add(GO);
                var newVector3 = GO.transform.position;
                newVector3.z = 2.5f;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                hitColliders.GetComponent<Level6Chest>().BusyPlaces ++;
                Destroy(gameObject);
                Level6Global.Victory --; 
            }
            else
            {
                transform.position = Position;
                Level6MouseMove.MoveFigures = null;
                gameObject.GetComponent<Level6StarMove>().State = 1;
            }
        }
        else
        {
            transform.position = Position;
            Level6MouseMove.MoveFigures = null;
            gameObject.GetComponent<Level6StarMove>().State = 1;
        }
    }
}