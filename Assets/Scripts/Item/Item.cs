using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Create Data/Item")]
public class Item : ScriptableObject
{
    public ItemData getItemDataObject;
    public ItemData getItemDataShadow;

    public void Setup(Transform transform)
    {
        getItemDataObject = Instantiate(getItemDataObject);
        getItemDataShadow = Instantiate(getItemDataShadow);

        getItemDataObject.Setup(transform);
        getItemDataShadow.Setup(transform);
    }
}
