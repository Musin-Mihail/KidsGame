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
        MouseMove.MoveFigures = gameObject;
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
                MouseMove.MoveFigures = null;
            }
        }
        else
        {
            transform.position = StartPosition;
            MouseMove.MoveFigures = null;
        }
    }
}