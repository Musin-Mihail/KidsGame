using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour 
{
    [Header("Handler Manager")]
    public HandlerManager GetHandlerManager;
    [Header("Winner Ponels")]
    public GameObject GetWinnerPonel;

    [Header("Handler Object and Shadow")]
    public List<GameObject> Object = new List<GameObject>();
    public List<GameObject> Shadow = new List<GameObject>();

    public List<ActorController> actorArray = new List<ActorController>();

    public void Add(ActorController actor)
    {
        actorArray.Add(actor);
        Object.Add(actor.GetItem.getItemDataObject.setGameObject);
        Shadow.Add(actor.GetItem.getItemDataShadow.setGameObject);
    }
    public void Remove(ActorController actor)
    {
        actorArray.Remove(actor);
        Object.Remove(actor.GetItem.getItemDataObject.setGameObject);
        Shadow.Remove(actor.GetItem.getItemDataShadow.setGameObject);
    }
}
