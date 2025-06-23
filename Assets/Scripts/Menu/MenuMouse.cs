using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMouse : MonoBehaviour
{
    public GameObject _protection;
    void OnMouseDown()
    {
        if( gameObject.name == "Answer")
        {
            _protection.GetComponent<Protection>().Payment();
            Debug.Log("Оплата");
            Invoke("Exit", 0.5f);
        }
        else
        {
            Debug.Log("Не угадал");
            Invoke("Exit", 0.5f);
        }
    }
    void Exit()
    {
        GetComponent<Protection>().Exit();
    }
}