using System.Collections;
using UnityEngine;

public class WinUp : MonoBehaviour
{
    public IEnumerator Win()
    {
        var startVector3 = transform.position;
        var topVector3 = transform.position;
        topVector3.y += 1;
        while (transform.position != topVector3)
        {
            transform.position = Vector3.MoveTowards(transform.position, topVector3, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }

        while (transform.position != startVector3)
        {
            transform.position = Vector3.MoveTowards(transform.position, startVector3, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
    }
}