using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Resolution : MonoBehaviour
{
    void Start()
    {
        if((Screen.width/16 - Screen.height/9) != 0)
        {
            var NewVector = transform.position;
            float correctionX = Screen.width;
            float correctionY = Screen.height;
            var Summa = (correctionX * correctionX) + (correctionY * correctionY);
            int i = Convert.ToInt32(Math.Sqrt(Summa));
            float qwe = (float)i / (float)2203;
            float CorrY = 0;
            if(Screen.height > 1080)
            {
                CorrY = correctionY/1080 - 1;
            }
            NewVector.x *= qwe-CorrY;
            transform.position = NewVector;
        }
    }
}