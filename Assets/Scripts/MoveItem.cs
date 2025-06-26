using System.Collections;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public int state = 1;
    public Vector3 startPosition;
    public Vector3 endPosition;

    public void Initialization(Vector3 start, Vector3 end)
    {
        startPosition = start;
        transform.position = start;
        endPosition = end;
    }

    public IEnumerator StartMove()
    {
        while (transform.position != endPosition && state == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, 0.02f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator Rotation()
    {
        while (state == 1)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Rotation());
    }
}