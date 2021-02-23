using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Item", menuName = "Create Data/Data")]
public class ItemData : ScriptableObject
{
    public enum GetTypeObject { Object, Shadow }
    public GetTypeObject getTypeObject;

    public GameObject setGameObject;

    public void Setup(Transform transform)
    {
        setGameObject = transform.Find(getTypeObject.ToString()).gameObject;
    }
}
