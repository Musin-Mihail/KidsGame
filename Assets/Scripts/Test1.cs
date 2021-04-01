using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{
    public Text Pay;
    void Start()
    {
        Pay.text += "Начало \n";
        string filePath = Application.persistentDataPath  + @"/open.all1";
        if(!File.Exists(filePath))
        {
            FileInfo fi = new FileInfo(filePath);
            fi.Create();
            Pay.text += "Файл создан \n";
            Pay.text += filePath + "\n";
        }
        else
        {
            Pay.text += "Файл уже есть \n";
            Pay.text += filePath + "\n";
        }
        Pay.text += "Конец \n";
    }
}
