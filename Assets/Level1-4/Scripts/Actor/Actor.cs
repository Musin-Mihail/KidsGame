using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour 
{
    public ActorMovement GetMovementController;
    public InputController GetInputController;
    public SortingLayerController GetSortingLayerController;

    void Awake()
    {
        GetMovementController = GetComponent<ActorMovement>();
        GetInputController = GetComponent<InputController>();
        GetSortingLayerController = GetComponent<SortingLayerController>();
    }

    void Start()
    {
        GetInputController.startPosition = transform.position;
        GetInputController.directionPosition = transform.position;
    }

    void Update()
    {
        GetMovementController.MoveTo(GetInputController.directionPosition);
    }
}