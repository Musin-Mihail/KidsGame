using UnityEngine;

public class Rotation360 : MonoBehaviour
{
    [Tooltip("Скорость вращения в градусах в секунду.")]
    [SerializeField] private float rotationSpeed = 20f;

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, -rotationSpeed * Time.deltaTime, Space.World);
    }
}