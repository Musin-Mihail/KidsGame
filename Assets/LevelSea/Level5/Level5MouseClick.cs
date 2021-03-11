using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5MouseClick : MonoBehaviour
{
    public Vector3 StartPosition;
    int layerMask = 1 << 6;
    void Start()
    {
       StartPosition = transform.position;
    }
    void OnMouseDown()
    {
        Level5Global.WaitHint = 1;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            
            if(hitColliders.tag == gameObject.tag)
            {
                hitColliders.GetComponent<SoundClickItem>().Play();
                var newVector3 = hitColliders.transform.position;
                newVector3.z += 0.5f;
                Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                Destroy(gameObject);
                hitColliders.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                hitColliders.tag = "Untagged";
                WinBobbles.Victory --;
                
            }
            else
            {
                transform.position = StartPosition;
            }
        }
        else
        {
            transform.position = StartPosition;
        }
    }
    void OnMouseDrag()
    {
        // if(Input.GetMouseButton(0))
        // {
        //     var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     _newVector2.z = 0;
        //     transform.position = _newVector2;
        // }
        if(Input.touchCount > 0)
        {
            var _newVector2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            _newVector2.z = 0;
            transform.position = _newVector2;
        }
    }
}