using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour 
{

    public enum controllerSearch { PATH, SEARCHOBJECT }
    public controllerSearch GetControllerSearch;

    public Color lineColor;
    public float radios;

    [SerializeField]public List<Transform> nodes;

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform[] pathTransform = GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i].transform != transform)
            {
                switch (GetControllerSearch)
                {
                    case controllerSearch.PATH:
                        if (pathTransform[i].name == "PathElement")
                            nodes.Add(pathTransform[i]);
                        break;
                    case controllerSearch.SEARCHOBJECT:
                        nodes.Add(pathTransform[i]);
                        break;
                    default:
                        break;
                }
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector2 currentNodes = nodes[i].position;
            Vector2 previusNodes = Vector2.zero;

            if (i > 0)
            {
                previusNodes = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previusNodes = nodes[nodes.Count-1].position;
            }

            Gizmos.DrawLine(previusNodes, currentNodes);
            Gizmos.DrawWireSphere(currentNodes, radios);
        }
    }
}
