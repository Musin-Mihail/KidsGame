using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2MouseClick : MonoBehaviour
{
    public Vector3 Position;
    public static GameObject MoveFigures;
    public Vector3 NormalScale;
    public Vector3 BigScale;

    int layerMask = 1 << 9;
    void Start() 
    {
        NormalScale = transform.localScale;
        BigScale = NormalScale + new Vector3(0.4f,0.4f,0);
        transform.localScale = BigScale;
    }

    void OnMouseDown()
    {
        Position = GetComponent<MoveItem>().StartPosition;
        Level2Global.WaitHint = 1;
        gameObject.GetComponent<MoveItem>().State = 0;
        transform.localScale = NormalScale;
    }
    void OnMouseUp()
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(transform.position, 0.1f, layerMask);
        if(hitColliders != null)
        {
            if(hitColliders.name == gameObject.name)
            {
                var newVector3 = hitColliders.transform.position;
                newVector3.z += 0.5f;
                // Instantiate(Resources.Load<ParticleSystem>("Bubbles"), newVector3, Quaternion.Euler(-90,-40,0));
                if (hitColliders.name == "Flag")
                {
                    hitColliders.GetComponent<Animator>().enabled = true;
                }
                else
                {
                    hitColliders.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                }
                hitColliders.GetComponent<SoundClickItem>().Play();
                Destroy(gameObject);
                WinBobbles.Victory --; 
            }
            else
            {
                transform.localScale = BigScale;
                transform.position = Position;
            }
        }
        else
        {
            transform.localScale = BigScale;
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