using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : Manager
{
    [Header("Path for object")]
    public Path path;

    void Awake()
    {
        GetHandlerManager = GetComponent<HandlerManager>();
        path = GameObject.FindObjectOfType<Path>();
    }

    void Update()
    {
        if (actorArray.Count <= 0)
        {
            GetWinnerPonel.SetActive(true);
            return;
        }

        GetHandlerManager.UpdateHandler(actorArray[0].GetItem);
        ControllerPosition();
    }

    private void ControllerPosition()
    {

        for (int i = 0; i < actorArray.Count; i++)
        {
            GameObject obj = actorArray[i].GetItem.getItemDataObject.setGameObject;
            obj.GetComponent<InputController>().SetPosition(path.nodes[i].transform.position);
        }
    }
}
