using UnityEngine;

[ExecuteInEditMode]
public class CameraAspectRatioMatcher : MonoBehaviour
{
    private const float TargetAspect = 16.0f / 9.0f;
    private Camera _camera;
    private int _lastScreenWidth;
    private int _lastScreenHeight;

    void Awake()
    {
#if UNITY_WEBGL
        enabled = false;
        if (TryGetComponent<Camera>(out var cam))
        {
            cam.rect = new Rect(0, 0, 1, 1);
        }
        return;
#endif

        _camera = GetComponent<Camera>();
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
        UpdateCameraRect();
    }

    void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
        {
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            UpdateCameraRect();
        }
    }

    /// <summary>
    /// Рассчитывает и устанавливает новую область отображения для камеры.
    /// </summary>
    private void UpdateCameraRect()
    {
        if (!_camera) return;

        var screenAspect = (float)Screen.width / (float)Screen.height;
        var scaleHeight = screenAspect / TargetAspect;

        if (scaleHeight < 1.0f)
        {
            var rect = new Rect(0, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            _camera.rect = rect;
        }
        else
        {
            var scaleWidth = 1.0f / scaleHeight;
            var rect = new Rect((1.0f - scaleWidth) / 2.0f, 0, scaleWidth, 1.0f);
            _camera.rect = rect;
        }
    }
}