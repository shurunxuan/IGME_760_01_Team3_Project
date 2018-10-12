using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class AStarUnit : Unit
{
    public GameObject GridLayout;
    public GameObject TargetPosition;


    private AStarGridLayout _grid;
    private Rigidbody _rigidbody;

    private bool _pathReady;

    private List<GridNode> _path;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        _pathReady = false;
        _path = new List<GridNode>();
        _grid = GridLayout.GetComponent<AStarGridLayout>();
    }

    void FixedUpdate()
    {
        MoveUnit(_rigidbody);
    }

    // Update is called once per frame
    void Update()
    {
        //if (_pathReady)
        //{

        //}
        //else
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            FindPath(TargetPosition.transform.position);
            timer.Stop();
            UnityEngine.Debug.Log(timer.ElapsedMilliseconds);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_pathReady)
        {
            foreach (var gridNode in _path)
            {
                Gizmos.DrawCube(gridNode.PositionInWorld, Vector3.one * 0.5f);
            }
        }
    }

    // This is the direction the unit will head to when GetMoveDirection().x > 0
    protected override void SetLocalRight()
    {
        localRight = Vector3.right;
    }

    // This is the direction the unit will head to when GetMoveDirection().y > 0
    protected override void SetLocalForward()
    {
        localForward = Vector3.forward;
    }

    protected override Vector2 GetMoveDirection()
    {
        // TODO: implement the algorithm
        return new Vector2(0, 0);
    }

    void FindPath(Vector3 targetPosition)
    {
        GridNode startNode = _grid.GetNodeFromPosition(transform.position);
        GridNode endNode = _grid.GetNodeFromPosition(targetPosition);

        Heap<GridNode> openSet = new Heap<GridNode>(_grid.MaxSize);
        HashSet<GridNode> closeSet = new HashSet<GridNode>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GridNode curNode = openSet.RemoveFirst();
            closeSet.Add(curNode);

            // Found target node
            if (curNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }

            // Find a node with minimal cost
            foreach (var item in _grid.GetNeighborhood(curNode))
            {
                if (!item.IsWalkable || closeSet.Contains(item))
                {
                    continue;
                }
                int newCost = curNode.GCost + getDistanceNodes(curNode, item);
                if (newCost < item.GCost || !openSet.Contains(item))
                {
                    item.GCost = newCost;
                    item.HCost = getDistanceNodes(item, endNode);
                    item.Parent = curNode;
                    if (!openSet.Contains(item))
                    {
                        openSet.Add(item);
                    }
                }
            }
        }

        GeneratePath(startNode, null);
        
    }

    void GeneratePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        if (endNode != null)
        {
            GridNode temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.Parent;
            }
            path.Reverse();
        }
        _pathReady = true;
        _path = path;
    }

    int getDistanceNodes(GridNode a, GridNode b)
    {
        //int cntX = Mathf.Abs(a.X - b.X);
        //int cntY = Mathf.Abs(a.Y - b.Y);

        ////if (cntX > cntY)
        ////{
        ////    return 14 * cntY + 10 * (cntX - cntY);
        ////}
        ////else
        ////{
        ////    return 14 * cntX + 10 * (cntY - cntX);
        ////}

        //return cntX + cntY;

        Vector3 diff = b.PositionInWorld - a.PositionInWorld;
        float horizontal = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        float vertical = diff.y;
        return Mathf.RoundToInt(100 * horizontal + vertical * (vertical > 0 ? 200 : 0));
    }


}
