using System.Collections;
using UnityEngine;

public class WinUp : MonoBehaviour
{
    [Tooltip("Скорость подъема и спуска объекта.")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("Высота, на которую 'подпрыгивает' объект.")]
    [SerializeField] private float jumpHeight = 1f;

    public IEnumerator Win()
    {
        var startVector3 = transform.position;
        var topVector3 = transform.position;
        topVector3.y += jumpHeight;

        while (Vector3.Distance(transform.position, topVector3) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, topVector3, moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(transform.position, startVector3) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startVector3, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = startVector3;
    }
}