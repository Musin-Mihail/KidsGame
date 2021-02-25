using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public static GameObject MoveFigures;
    void Update()
    {
        if(MoveFigures != null)
        {
            if(Input.GetMouseButton(0))
            {
                var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _newVector2.z = 0;
                MoveFigures.transform.position = _newVector2;
            }
            else if(Input.touchCount > 0)
            {
                var _newVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _newVector2.z = 0;
                MoveFigures.transform.position = _newVector2;
            }
        }
    }
}