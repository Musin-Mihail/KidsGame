using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Spawn : MonoBehaviour
{
    public List<Transform> SpawnPositionVector;
    public List<GameObject> SpawnPosition;
    public Transform _scale;
    public Transform _scaleWindows3;
    public Transform _scaleWindows1;
    void Start()
    {
        SpawnPosition = new List<GameObject>();
        foreach (var item in SpawnPositionVector)
        {
            var _item = Instantiate (Level2Global.AllItemStatic[0], item.position, Quaternion.identity);
            _item.name = Level2Global.AllItemStatic[0].name;
            if(_item.name == "Window3")
            {
                _item.transform.localScale = _scaleWindows3.transform.lossyScale*1.5f;
            }
            else if (_item.name == "Window1")
            {
                _item.transform.localScale = _scaleWindows1.transform.lossyScale*1.5f;
            }
            else
            {
                _item.transform.localScale = _scale.transform.lossyScale*1.5f;
            }
            SpawnPosition.Add(_item);
            Level2Global.AllItemStatic.RemoveAt(0);
        }
    }
}