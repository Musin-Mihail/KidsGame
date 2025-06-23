using System;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    private void Start()
    {
        if ((Screen.width / 16 - Screen.height / 9) != 0)
        {
            var newVector = transform.position;
            float correctionX = Screen.width;
            float correctionY = Screen.height;
            var summa = (correctionX * correctionX) + (correctionY * correctionY);
            var i = Convert.ToInt32(Math.Sqrt(summa));
            var qwe = (float)i / (float)2203;
            float CorrY = 0;
            if (Screen.height > 1080)
            {
                CorrY = correctionY / 1080 - 1;
            }

            newVector.x *= qwe - CorrY;
            transform.position = newVector;
        }
    }
}