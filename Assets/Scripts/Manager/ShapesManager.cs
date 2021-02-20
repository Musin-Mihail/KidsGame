using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : Manager
{

    [Header("Path for object")]
    public Path path;

    [Header("Path for object")]
    public PetsController GetPetsController;

    void Awake()
    {
        GetHandlerManager = GetComponent<HandlerManager>();

        GetPetsController = GameObject.FindObjectOfType<PetsController>();

        path = GameObject.FindObjectOfType<Path>();
    }

    void Update()
    {
        if (GetPetsController.actors.Count <= 0)
        {
            GetWinnerPonel.SetActive(true);
            return;
        }

        if (actorArray.Count <= 0)
            return;


        GetHandlerManager.UpdateHandler(GetPetsController.GetActor().controllers[0].GetItem);
        //ControllerPosition();
    }

    private void ControllerPosition()
    {
        for (int i = 0; i < actorArray.Count; i++)
        {
            if (actorArray[i] == null)
            {
                actorArray.RemoveAt(i);
                Object.RemoveAt(i);
                Shadow.RemoveAt(i);

                if (actorArray.Count <= 0)
                {
                    GetWinnerPonel.SetActive(true);
                }
                continue;
            }

            GameObject obj = actorArray[i].GetItem.getItemDataObject.setGameObject;
            obj.GetComponent<InputController>().SetPosition(path.nodes[i].transform.position);
        }
    }
}
