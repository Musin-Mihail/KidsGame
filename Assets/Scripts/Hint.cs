using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public List<GameObject> itemPositions;
    public List<GameObject> emptyItemPositions;
    public GameObject finger;
    public int waitHint;
    private int _hintTime;

    public void Initialization(List<GameObject> newEmptyItemPositions, List<GameObject> newItemPositions)
    {
        emptyItemPositions = newEmptyItemPositions;
        itemPositions = newItemPositions;
    }

    public IEnumerator StartHint()
    {
        while (WinBobbles.instance.victory != 0)
        {
            while (_hintTime < 4)
            {
                yield return new WaitForSeconds(1.0f);
                if (waitHint == 1)
                {
                    _hintTime = 0;
                    waitHint = 0;
                    break;
                }

                _hintTime++;
            }

            if (_hintTime >= 4)
            {
                StartCoroutine(Search());
            }

            _hintTime = 0;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator Search()
    {
        finger.gameObject.SetActive(true);
        var target = new Vector3(0, 10, 0);
        var check = 0;
        var itemName = "";

        foreach (var item in itemPositions.Where(item => item.activeSelf))
        {
            itemName = item.name;
            finger.transform.position = item.transform.position;
            check = 1;
            break;
        }

        if (check == 1)
        {
            foreach (var item in emptyItemPositions.Where(item => itemName == item.name))
            {
                check = 2;
                target = item.transform.position;
                target.z += -1;
                break;
            }
        }

        if (check != 2) yield break;
        finger.gameObject.SetActive(true);
        while (finger.transform.position != target)
        {
            finger.transform.position = Vector3.MoveTowards(finger.transform.position, target, GameConstants.HintDistance);
            if (waitHint == 1)
            {
                break;
            }

            yield return new WaitForSeconds(GameConstants.HintSpeed);
        }
        finger.gameObject.SetActive(false);
    }
}