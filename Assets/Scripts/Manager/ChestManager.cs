using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : Manager
{
    public List<Shell> GetShells = new List<Shell>();

    public Chest GetChest;

    void Awake()
    {
        GetHandlerManager = GetComponent<HandlerManager>();
        GetShells.AddRange(GameObject.FindObjectsOfType<Shell>());
    }

    void Start()
    {
        StartCoroutine(UpdateScene());
    }

    IEnumerator UpdateScene()
    {
        while (GetShells.Count > 0)
        {
            if (GetShells[0].actorArray[0])
                GetHandlerManager.UpdateHandler(GetShells[0].actorArray[0].GetComponentInParent<ActorController>().GetItem);

            yield return null;
        }

        yield return StartCoroutine(GetChest.DisableShell());
        yield return StartCoroutine(GetChest.DisableObject());
         
        yield return new WaitForSeconds(1f);

        GetWinnerPonel.SetActive(true);

    }

    void Updates()
    {
        if (GetShells.Count <= 0)
        {
            GetWinnerPonel.SetActive(true);
            GetChest.DisableObject();
            return;
        }

        if (GetShells[0].actorArray[0])
            GetHandlerManager.UpdateHandler(GetShells[0].actorArray[0].GetComponentInParent<ActorController>().GetItem);
    }
}
