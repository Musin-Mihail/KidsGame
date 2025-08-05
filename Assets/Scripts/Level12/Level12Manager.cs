using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Level12
{
    /// <summary>
    /// Управляет игровой логикой для 12-го уровня.
    /// Наследуется от BaseLevelManager для использования общей логики уровней и синглтона.
    /// </summary>
    public class Level12Manager : BaseLevelManager<Level12Manager>
    {
        [Header("Настройки уровня 12")]
        [Tooltip("Главный объект-контейнер для целей, который анимируется в начале")]
        [SerializeField] private GameObject targetContainer;
        private int _currentTaskIndex;
        private GameObject _hintStartObject;
        private readonly Dictionary<GameObject, Vector3> _initialScales = new();

        protected override void Awake()
        {
            base.Awake();
            _hintStartObject = new GameObject("HintStartObject_Permanent")
            {
                transform = { position = new Vector3(0, -6, 0) }
            };
        }

        protected override void Start()
        {
            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory = allTargets.Count;
            }

            foreach (var target in allTargets.Where(target => target))
            {
                _initialScales[target] = target.transform.localScale;
            }

            Shuffle(allTargets);
            StartCoroutine(GameFlow());
        }

        private void OnDestroy()
        {
            if (_hintStartObject)
            {
                Destroy(_hintStartObject);
            }
        }

        /// <summary>
        /// Основная корутина, управляющая началом игры и анимациями.
        /// </summary>
        private IEnumerator GameFlow()
        {
            yield return new WaitForSeconds(5.5f);
            if (targetContainer && targetContainer.TryGetComponent<Animator>(out var animator))
            {
                animator.enabled = false;
            }

            ShowNextTarget();
            InitializeHint();
            StartCoroutine(hint.StartHint());
        }

        /// <summary>
        /// Обрабатывает клик по предмету. Вызывается из LevelDropHandler.
        /// </summary>
        public void ProcessClick(GameObject clickedItem)
        {
            if (_currentTaskIndex >= allTargets.Count) return;
            var currentTarget = allTargets[_currentTaskIndex];
            if (clickedItem.name != currentTarget.name) return;
            if (hint) hint.waitHint = 1;
            OnCorrectItemClicked(clickedItem);
        }

        /// <summary>
        /// Вызывается при клике на правильный предмет.
        /// </summary>
        private void OnCorrectItemClicked(GameObject clickedItem)
        {
            var currentTarget = allTargets[_currentTaskIndex];
            if (clickedItem.TryGetComponent<Collider2D>(out var collider))
            {
                collider.enabled = false;
            }

            StartCoroutine(MoveAndScaleItem(clickedItem, currentTarget));
        }

        /// <summary>
        /// Корутина для перемещения и масштабирования предмета к цели.
        /// </summary>
        private IEnumerator MoveAndScaleItem(GameObject item, GameObject target)
        {
            AudioManager.instance.PlayClickSound();
            Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), item.transform.position, Quaternion.Euler(-90, 0, 0));
            if (item.TryGetComponent<SpriteRenderer>(out var itemRenderer))
            {
                itemRenderer.sortingOrder = 12;
            }

            if (target.TryGetComponent<Animator>(out var targetAnimator))
            {
                targetAnimator.enabled = false;
            }

            if (_initialScales.TryGetValue(target, out var initialScale))
            {
                target.transform.localScale = initialScale;
            }

            var moveCoroutine = StartCoroutine(Move(item, target.transform.position));
            var scaleCoroutine = StartCoroutine(Scale(item, target.transform.localScale));
            yield return moveCoroutine;
            yield return scaleCoroutine;
            Instantiate(Resources.Load<ParticleSystem>("ParticleSrarsLevel11"), item.transform.position, Quaternion.Euler(-90, 0, 0));
            if (target.TryGetComponent<SpriteRenderer>(out var targetRenderer))
            {
                targetRenderer.enabled = false;
            }

            if (WinBobbles.instance)
            {
                WinBobbles.instance.victory--;
            }

            _currentTaskIndex++;
            if (_currentTaskIndex < allTargets.Count)
            {
                ShowNextTarget();
                InitializeHint();
            }
            else
            {
                StartCoroutine(WinAnimation());
            }
        }

        private IEnumerator Move(GameObject item, Vector3 targetPosition)
        {
            const float speed = 10f;
            while (Vector3.Distance(item.transform.position, targetPosition) > 0.01f)
            {
                item.transform.position = Vector2.MoveTowards(item.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            item.transform.position = targetPosition;
        }

        private IEnumerator Scale(GameObject item, Vector3 targetScale)
        {
            const float speed = 0.8f;
            while (Vector3.Distance(item.transform.localScale, targetScale) > 0.01f)
            {
                item.transform.localScale = Vector3.MoveTowards(item.transform.localScale, targetScale, speed * Time.deltaTime);
                yield return null;
            }

            item.transform.localScale = targetScale;
        }

        /// <summary>
        /// Победная анимация в конце уровня.
        /// </summary>
        private IEnumerator WinAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            if (hint)
            {
                hint.StopAllCoroutines();
                if (hint.finger) hint.finger.SetActive(false);
            }

            foreach (var item in allItems.Where(i => i && i.activeSelf))
            {
                StartCoroutine(AnimateWinItem(item));
                yield return new WaitForSeconds(0.02f);
            }
        }

        /// <summary>
        /// Анимация "подпрыгивания" для одного предмета.
        /// </summary>
        private IEnumerator AnimateWinItem(GameObject item)
        {
            var originalScale = item.transform.localScale;
            var bigScale = originalScale * 1.2f;
            const float duration = 0.2f;
            yield return AnimateScale(item, originalScale, bigScale, duration);
            yield return AnimateScale(item, bigScale, originalScale, duration);
        }

        private IEnumerator AnimateScale(GameObject item, Vector3 from, Vector3 to, float duration)
        {
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                if (!item) yield break;
                item.transform.localScale = Vector3.Lerp(from, to, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (item)
            {
                item.transform.localScale = to;
            }
        }

        /// <summary>
        /// Показывает следующую цель, включая анимацию.
        /// </summary>
        private void ShowNextTarget()
        {
            if (_currentTaskIndex > 0)
            {
                var prevTarget = allTargets[_currentTaskIndex - 1];
                if (prevTarget && prevTarget.TryGetComponent<Animator>(out var prevAnimator))
                {
                    prevAnimator.Play("Empty");
                }
            }

            var currentTarget = allTargets[_currentTaskIndex];
            if (currentTarget && currentTarget.TryGetComponent<Animator>(out var currentAnimator))
            {
                currentAnimator.Play("Scale");
            }
        }

        protected override void InitializeSpawner()
        {
            // На этом уровне объекты уже размещены на сцене, спаунер не нужен.
        }

        /// <summary>
        /// Инициализация подсказки.
        /// </summary>
        protected override void InitializeHint()
        {
            if (!hint || _currentTaskIndex >= allTargets.Count) return;
            var currentTarget = allTargets[_currentTaskIndex];
            var correctItem = allItems.FirstOrDefault(item => item && item.name == currentTarget.name && item.GetComponent<Collider2D>()?.enabled == true);
            if (correctItem && _hintStartObject)
            {
                _hintStartObject.name = correctItem.name;
                hint.comparisonType = HintComparisonType.ByName;
                hint.Initialization(new List<GameObject> { correctItem }, new List<GameObject> { _hintStartObject });
            }

            if (hint) hint.waitHint = 1;
        }
    }
}