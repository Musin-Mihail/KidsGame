using System.Collections.Generic;
using UnityEngine;

namespace Level6
{
    public class Level6Chest : MonoBehaviour
    {
        [Tooltip("Список заранее созданных объектов-звёзд. Назначьте их в инспекторе.")]
        public List<GameObject> starPlaceholders = new();

        [HideInInspector] public int busyPlaces;
    }
}