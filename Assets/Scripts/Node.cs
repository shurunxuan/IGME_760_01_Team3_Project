using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Nodes structure
public class Node : MonoBehaviour
{

    [HideInInspector]
    public List<Node> neighbors = new List<Node>();

    void Awake()
    {
        FindNeighbors();
    }

    //Create a network of nodes
    public void FindNeighbors()
    {
        neighbors = new List<Node>();

        foreach (Node n in GameObject.FindObjectsOfType<Node>())
        {
            Ray ray = new Ray(transform.position, n.transform.position - transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == n.transform)
                {
                    neighbors.Add(n);
                    //Debug.DrawLine(transform.position, n.transform.position, Color.yellow, 6);
                }
                else
                {
                    continue;
                }
            }
        }
    }


    //Calculate f, g and h costs
    public int fCost(Node start, Node end)
    {
        return gCost(start) + hCost(end);
    }

    public int gCost(Node start)
    {
        return Mathf.RoundToInt(Vector3.Distance(transform.position, start.transform.position));
    }

    public int hCost(Node end)
    {
        return Mathf.RoundToInt(Vector3.Distance(transform.position, end.transform.position));
    }
}
