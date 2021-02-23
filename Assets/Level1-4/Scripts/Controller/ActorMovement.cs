using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovement : MonoBehaviour 
{
    public float speed;

    public void MoveTo(Vector2 direction)
    {
        Vector2 pos = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        transform.position = pos;
    }
}
