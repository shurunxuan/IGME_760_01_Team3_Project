using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //Dictionary to hold the parents(previous nodes)
    Dictionary<Node, Node> parentNodes = new Dictionary<Node, Node>();

    //Calculate the route to the destination
    public List<Vector3> CalculatePath(Node start, Node end)
    {
        
        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();
        openSet.Add(start);
        Node currentNode = openSet[0];

        while (openSet.Count > 0)
        {
            currentNode = openSet[0];
            start.FindNeighbors();
            end.FindNeighbors();

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost(start, end) < currentNode.fCost(start, end) || openSet[i].fCost(start,end) == currentNode.fCost(start,end) && openSet[i].hCost(end) < currentNode.hCost(end) )
                {
                    currentNode = openSet[i];
                }
            }

            if (currentNode == end)
            {
                print("Found Path");
                return reconstructPath(start,end);
            }

            closedSet.Add(currentNode);
            openSet.Remove(currentNode);

            //calculate costs for neighboring nodes
            foreach (Node n in currentNode.neighbors)
            {
                if (closedSet.Contains(n))
                {
                    continue;
                }
                int tGCost = currentNode.gCost(start) + distBetween(n, currentNode);

                if (!openSet.Contains(n))
                {
                    openSet.Add(n);
                }
                else if (tGCost >= n.gCost(start))
                {
                    continue;
                }

                if (parentNodes.ContainsKey(n))
                {
                    parentNodes[n] = currentNode;
                }
                else
                {
                    parentNodes.Add(n, currentNode);
                }
            }
        }

        print("Path not found");
        return null;
    }

    //Function to calculate distance between the nodes
    int distBetween(Node a, Node b)
    {
        return Mathf.RoundToInt(Vector3.Distance(a.transform.position, b.transform.position));
    }


    //Backtrack to the start node from the end(last node)
    List<Vector3> reconstructPath(Node start, Node end)
    {
        List<Vector3> curPath = new List<Vector3>();
        Node currentNode = end;
        while (currentNode != start)
        {
            curPath.Add(currentNode.transform.position);
            currentNode = parentNodes[currentNode];

        }

        if (!curPath.Contains(start.transform.position))
        {
            curPath.Add(start.transform.position);
        }

        curPath.Reverse();
        return curPath;
    }

    //FInd all neighbors for the current node
    public void FindAllNeighbors()
    {
        foreach (Node n in GameObject.FindObjectsOfType<Node>())
        {
            n.FindNeighbors();
        }
    }


}
