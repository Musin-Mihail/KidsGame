using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// Абстрактный базовый класс для всех менеджеров уровней.
    /// </summary>
    public abstract class BaseLevelManager<T> : Singleton<T> where T : MonoBehaviour
    {
        [Header("Общие настройки уровня")]
        [Tooltip("Компонент подсказки на этом же объекте")]
        [SerializeField] protected Hint hint;

        [Tooltip("Список всех перетаскиваемых предметов на уровне (животные, фигуры и т.д.)")]
        public List<GameObject> allItems = new();

        [Tooltip("Список всех целевых пустых мест на уровне")]
        public List<GameObject> allTargets = new();

        protected override void Awake()
        {
            base.Awake();
            if (!hint) hint = GetComponent<Hint>();
        }

        protected virtual void Start()
        {
            if (WinBobbles.instance != null)
            {
                WinBobbles.instance.victory = allTargets.Count;
            }

            Shuffle(allItems);

            InitializeSpawner();
            InitializeHint();
        }

        /// <summary>
        /// Перемешивает элементы в списке.
        /// </summary>
        protected void Shuffle<T_List>(List<T_List> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }

        protected abstract void InitializeSpawner();
        protected abstract void InitializeHint();
    }
}