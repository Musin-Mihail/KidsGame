using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUp : MonoBehaviour
{
    public IEnumerator Win()
    {
        Vector3 StartVector3 = transform.position;
        Vector3 TopVector3 = transform.position;
        TopVector3.y +=1;
        while (transform.position !=TopVector3)
        {
            transform.position = Vector3.MoveTowards(transform.position, TopVector3, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        while (transform.position !=StartVector3)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartVector3, 0.1f);
            yield return new WaitForSeconds(0.02f);
        } 
    }
}