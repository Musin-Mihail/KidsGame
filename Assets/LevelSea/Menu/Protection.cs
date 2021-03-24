using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protection : MonoBehaviour
{
    public GameObject _protection;
    public List<GameObject> _protectionList;
    public void Exit()
    {
        _protection.SetActive(false);
    }
    public void Open()
    {
        int _random = Random.Range(0,4);
        _protection.SetActive(true);
        for (int i = 0; i < _protectionList.Count; i++)
        {
            if(i == _random)
            {
                _protectionList[i].SetActive(true);
            }
            else
            {
                _protectionList[i].SetActive(false);
            }
        }
    }
}
