using System.Collections;
using UnityEngine;

namespace Level8
{
    public class Level8MoveItem : MonoBehaviour
    {
        public GameObject _game;

        public IEnumerator Move(int count)
        {
            var target = transform.parent.gameObject.GetComponent<Level8>().allSpawn[count].transform.position;
            _game.GetComponent<Level8Mouse>().position = target;
            gameObject.name = transform.parent.gameObject.GetComponent<Level8>().allPlace[count].name;
            while (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, 0.05f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}