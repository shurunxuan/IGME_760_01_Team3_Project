using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AStarPath : MonoBehaviour
{
    Pathfinding manager;
    public Node end;
    public float closeEnough;
    public float movementSpeed;
    List<Vector3> path = new List<Vector3>();

    void FixedUpdate()
    {

        manager = GameObject.FindObjectOfType<Pathfinding>();
        manager.FindAllNeighbors();

        path = manager.CalculatePath(this.GetComponent<Node>(), end);
        StartFollowing();
    }

    //Move the unit along the path
    void StartFollowing()
    {
        StopAllCoroutines();
        reached = true;
        StartCoroutine("FollowPath");
    }

    bool reached;

   
    IEnumerator FollowPath()
    {
        if (path != null && path.Count > 0)
        {
            for (int i = 0; i < path.Count; i++)
            {
                reached = false;
                while (reached != true)
                {
                    if (Vector3.Distance(transform.position, path[i]) <= closeEnough)
                    {
                        reached = true;
                    }
                    else
                    {
                        Vector3 dir = Vector3.Normalize(path[i] - transform.position) * movementSpeed;
                        transform.position += dir;
                    }
                    yield return null;
                }
            }

        }
    }


    //Draw path to the destination
   void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (path != null)
        {
            if(path.Count > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //Debug.DrawLine(path[i], path[i-1], Color.blue, 5);
                        Gizmos.DrawLine(path[i], path[i - 1]);
                    }
                }
            }
        }

    }


}
