using System.Collections.Generic;
using UnityEngine;

public class CalculateEdgePositions
{
    public (List<Vector3> side1, List<Vector3> side2)? Calculate(Camera mainCamera, List<Transform> spawnPositions)
    {
        if (!mainCamera)
        {
            Debug.LogError("Основная камера не найдена. Убедитесь, что у вас есть камера с тегом 'MainCamera'.");
            return null;
        }

        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogWarning("Список 'spawnPositions' не заполнен. Невозможно вычислить позиции.");
            return null;
        }

        var rightEdgeWorldPoint = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        var side1 = new List<Vector3>();
        var side2 = new List<Vector3>();
        foreach (var spawnTransform in spawnPositions)
        {
            if (spawnTransform == null)
            {
                Debug.LogWarning("В списке 'spawnPositions' есть пустой элемент. Он будет пропущен.");
                continue;
            }

            var yPos = spawnTransform.position.y;
            var zPos = spawnTransform.position.z;
            var leftPoint = new Vector3(rightEdgeWorldPoint.x - GameConstants.SpawnEdgeOffset, yPos, zPos);
            side1.Add(leftPoint);
            var rightPoint = new Vector3(rightEdgeWorldPoint.x + GameConstants.SpawnEdgeOffset, yPos, zPos);
            side2.Add(rightPoint);
        }

        return (side1, side2);
    }
}