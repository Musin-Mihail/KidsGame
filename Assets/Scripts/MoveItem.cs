using System.Collections;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    [HideInInspector] public bool isMoving = true;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 endPosition;
    [HideInInspector] public float speed;
    private readonly WaitForSeconds _rotationDelay = new(0.05f);

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
    /// Движение происходит, пока 'isMoving' равно true.
    /// </summary>
    public IEnumerator Move()
    {
        if (speed <= 0)
        {
            yield break;
        }

        while (Vector3.Distance(transform.position, endPosition) > 0.01f && isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            yield return null;
        }

        if (isMoving)
        {
            transform.position = endPosition;
        }
    }

    /// <summary>
    /// Корутина для вращения объекта.
    /// Вращение происходит, пока 'isMoving' равен true.
    /// </summary>
    public IEnumerator Rotation()
    {
        while (isMoving)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 5);
            yield return _rotationDelay;
        }
    }
}