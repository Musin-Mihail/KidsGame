using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    public Vector2 targetResolution = new(1920, 1080);
    public float pixelsPerUnit = 100f;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateCameraSize();
    }

    private void UpdateCameraSize()
    {
        if (!_camera) return;
        if (!_camera.orthographic)
        {
            Debug.LogWarning("Камера не является ортографической! Скрипт CameraScaler работает только с ортографическими камерами.");
            return;
        }

        var targetOrthographicSize = targetResolution.y / 2 / pixelsPerUnit;
        var targetAspect = targetResolution.x / targetResolution.y;
        var currentAspect = (float)Screen.width / (float)Screen.height;

        if (currentAspect >= targetAspect)
        {
            _camera.orthographicSize = targetOrthographicSize;
        }
        else
        {
            var newOrthographicSize = targetOrthographicSize * (targetAspect / currentAspect);
            _camera.orthographicSize = newOrthographicSize;
        }
    }
}