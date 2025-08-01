using System.Collections;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    [HideInInspector] public int state = 1;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 endPosition;
    [HideInInspector] public float speed;

    /// <summary>
    /// Инициализирует начальные параметры для движения.
    /// </summary>
    /// <param name="start">Начальная позиция.</param>
    /// <param name="end">Конечная позиция.</param>
    /// <param name="moveSpeed">Скорость движения в юнитах/сек.</param>
    public void Initialization(Vector3 start, Vector3 end, float moveSpeed)
    {
        startPosition = start;
        transform.position = start;
        endPosition = end;
        speed = moveSpeed;
    }

    /// <summary>
    /// Корутина для плавного перемещения объекта из startPosition в endPosition.
    /// Движение происходит, пока 'state' равен 1.
    /// </summary>
    public IEnumerator Move()
    {
        if (speed <= 0)
        {
            yield break;
        }

        while (Vector3.Distance(transform.position, endPosition) > 0.01f && state == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            yield return null;
        }

        if (state == 1)
        {
            transform.position = endPosition;
        }
    }

    /// <summary>
    /// Корутина для вращения объекта.
    /// Вращение происходит, пока 'state' равен 1.
    /// </summary>
    public IEnumerator Rotation()
    {
        while (state == 1)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 5);
            yield return new WaitForSeconds(0.05f);
        }
    }
}